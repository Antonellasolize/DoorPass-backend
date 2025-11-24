using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Compras;
using ReservasDiscoteca.API.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Usuario")] // ¡SOLO USUARIOS!
    public class ComprasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ComprasController(AppDbContext context) { _context = context; }

        // ... (Los métodos ComprarManillas, ReservarMesa, ComprarCombos y CancelarCompra
        //      se quedan exactamente igual que antes... no los pego para no hacer bulto) ...


        // --- ACCIÓN 1: COMPRAR MANILLAS ---
        [HttpPost("manillas")]
        public async Task<ActionResult<DetalleCompraDto>> ComprarManillas(CrearCompraManillasDto compraDto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal totalPagado = 0;
                var boliche = await _context.Boliches.FindAsync(compraDto.BolicheId);
                if (boliche == null) return NotFound("Boliche no encontrado.");

                var nuevaCompra = new Compra
                {
                    UsuarioId = usuarioId,
                    BolicheId = compraDto.BolicheId,
                    TipoCompra = "Manillas"
                };

                var manillasCompradas = new List<CompraManilla>();
                foreach (var item in compraDto.Manillas)
                {
                    var manillaTipo = await _context.ManillaTipos.FindAsync(item.ManillaTipoId);
                    if (manillaTipo == null) throw new Exception("Manilla no encontrada.");
                    if (manillaTipo.BolicheId != boliche.Id) throw new Exception("Manilla no pertenece a este boliche.");
                    if (manillaTipo.Stock < item.Cantidad) throw new Exception($"No hay stock para '{manillaTipo.Nombre}'.");

                    manillaTipo.Stock -= item.Cantidad;
                    totalPagado += manillaTipo.Precio * item.Cantidad;
                    manillasCompradas.Add(new CompraManilla
                    {
                        Compra = nuevaCompra, ManillaTipoId = item.ManillaTipoId, Cantidad = item.Cantidad, PrecioEnElMomento = manillaTipo.Precio
                    });
                }

                nuevaCompra.TotalPagado = totalPagado;
                nuevaCompra.ManillasCompradas = manillasCompradas;
                _context.Compras.Add(nuevaCompra);
                
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
                return await GetDetalleCompraDto(nuevaCompra.Id);
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // --- ACCIÓN 2: RESERVAR MESA ---
        [HttpPost("mesas")]
        public async Task<ActionResult<DetalleCompraDto>> ReservarMesa(ReservarMesaDto reservaDto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var boliche = await _context.Boliches.FindAsync(reservaDto.BolicheId);
                if (boliche == null) return NotFound("Boliche no encontrado.");
                
                var mesa = await _context.Mesas.FindAsync(reservaDto.MesaId);
                if (mesa == null) throw new Exception("Mesa no encontrada.");
                if (mesa.BolicheId != boliche.Id) throw new Exception("Mesa no pertenece a este boliche.");
                if (!mesa.EstaDisponible) throw new Exception($"La mesa '{mesa.NombreONumero}' ya no está disponible.");

                var nuevaCompra = new Compra
                {
                    UsuarioId = usuarioId,
                    BolicheId = reservaDto.BolicheId,
                    TipoCompra = "Reserva de Mesa",
                    TotalPagado = mesa.PrecioReserva
                };
                _context.Compras.Add(nuevaCompra);
                
                mesa.EstaDisponible = false;
                mesa.Compra = nuevaCompra; 
                
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
                return await GetDetalleCompraDto(nuevaCompra.Id);
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // --- ACCIÓN 3: COMPRAR COMBOS ---
        [HttpPost("combos")]
        public async Task<ActionResult<DetalleCompraDto>> ComprarCombos(CrearCompraCombosDto compraDto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal totalPagado = 0;
                var boliche = await _context.Boliches.FindAsync(compraDto.BolicheId);
                if (boliche == null) return NotFound("Boliche no encontrado.");

                var nuevaCompra = new Compra
                {
                    UsuarioId = usuarioId,
                    BolicheId = compraDto.BolicheId,
                    TipoCompra = "Combos"
                };

                var combosComprados = new List<CompraCombo>();
                foreach (var item in compraDto.Combos)
                {
                    var combo = await _context.Combos.FindAsync(item.ComboId);
                    if (combo == null) throw new Exception("Combo no encontrado.");
                    if (combo.BolicheId != boliche.Id) throw new Exception("Combo no pertenece a este boliche.");

                    totalPagado += combo.Precio * item.Cantidad;
                    combosComprados.Add(new CompraCombo
                    {
                        Compra = nuevaCompra, ComboId = item.ComboId, Cantidad = item.Cantidad, PrecioEnElMomento = combo.Precio
                    });
                }

                nuevaCompra.TotalPagado = totalPagado;
                nuevaCompra.CombosComprados = combosComprados;
                _context.Compras.Add(nuevaCompra);
                
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
                return await GetDetalleCompraDto(nuevaCompra.Id);
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // --- HISTORIAL DEL PROPIO USUARIO (MÉTODO CORREGIDO) ---
        [HttpGet("mi-historial")]
        public async Task<ActionResult<List<DetalleCompraDto>>> GetMiHistorial()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // --- ESTA ES LA PARTE CORREGIDA ---

            // 1. Traer los datos "crudos" de la BD a la memoria
            var historialCrudo = await _context.Compras
                .Where(c => c.UsuarioId == usuarioId) // Filtramos por el usuario
                .AsNoTracking()
                .Include(c => c.Boliche)
                .Include(c => c.Usuario)
                .Include(c => c.MesaReservada)
                .Include(c => c.ManillasCompradas).ThenInclude(mc => mc.ManillaTipo)
                .Include(c => c.CombosComprados).ThenInclude(cc => cc.Combo)
                .OrderByDescending(h => h.FechaCompra) // Ordenamos en la BD
                .ToListAsync(); // <-- ¡Traemos a memoria!

            // 2. Ahora que están en memoria, los mapeamos usando C#
            var historialDto = historialCrudo
                .Select(c => MapToDetalleCompraDto(c))
                .ToList();

            return Ok(historialDto);
            // ------------------------------------
        }

        // --- ACCIÓN 4: CANCELAR UNA COMPRA/RESERVA ---
        [HttpPatch("cancelar/{compraId}")]
        public async Task<IActionResult> CancelarCompra(int compraId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var compra = await _context.Compras
                    .Include(c => c.ManillasCompradas)
                    .Include(c => c.MesaReservada)
                    .FirstOrDefaultAsync(c => c.Id == compraId && c.UsuarioId == usuarioId);

                if (compra == null) return NotFound("Compra no encontrada o no te pertenece.");
                if (!compra.EstaActiva) return BadRequest("Esta compra ya ha sido cancelada.");
                
                // 1. Marca la compra como inactiva
                compra.EstaActiva = false;
                
                // 2. Lógica de "Devolución"
                if (compra.TipoCompra == "Manillas")
                {
                    // Devolver el stock
                    foreach (var item in compra.ManillasCompradas)
                    {
                        var manillaTipo = await _context.ManillaTipos.FindAsync(item.ManillaTipoId);
                        if (manillaTipo != null)
                        {
                            manillaTipo.Stock += item.Cantidad;
                        }
                    }
                }
                else if (compra.TipoCompra == "Reserva de Mesa")
                {
                    // Devolver la mesa a "Disponible"
                    if (compra.MesaReservada != null)
                    {
                        compra.MesaReservada.EstaDisponible = true;
                        compra.MesaReservada.CompraId = null;
                    }
                }
                // (Los combos no tienen stock, no se devuelve nada)
                
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
                
                return Ok(new { message = "Compra cancelada y stock/mesa devuelto correctamente." });
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return BadRequest($"Error al cancelar: {ex.Message}");
            }
        }
        
        // --- MÉTODO HELPER PRIVADO (Este estaba bien) ---
        
        private async Task<DetalleCompraDto> GetDetalleCompraDto(int compraId)
        {
            var c = await _context.Compras.AsNoTracking()
                .Include(c => c.Boliche).Include(c => c.Usuario)
                .Include(c => c.MesaReservada)
                .Include(c => c.ManillasCompradas).ThenInclude(mc => mc.ManillaTipo)
                .Include(c => c.CombosComprados).ThenInclude(cc => cc.Combo)
                .FirstOrDefaultAsync(c => c.Id == compraId);

            return MapToDetalleCompraDto(c);
        }

        private DetalleCompraDto MapToDetalleCompraDto(Compra c)
        {
            return new DetalleCompraDto
            {
                CompraId = c.Id,
                FechaCompra = c.FechaCompra,
                TotalPagado = c.TotalPagado,
                TipoCompra = c.TipoCompra,
                EstaActiva = c.EstaActiva,
                BolicheId = c.Boliche.Id,
                NombreBoliche = c.Boliche.Nombre,
                UsuarioId = c.Usuario.Id,
                NombreUsuario = c.Usuario.Nombre,
                EmailUsuario = c.Usuario.Email,
                MesaReservada = c.MesaReservada != null
                    ? $"{c.MesaReservada.NombreONumero} - {c.MesaReservada.Ubicacion}" : null,
                ManillasCompradas = c.ManillasCompradas.Select(mc => new ItemCompradoDto
                {
                    NombreManilla = mc.ManillaTipo.Nombre, Cantidad = mc.Cantidad, PrecioPagado = mc.PrecioEnElMomento
                }).ToList(),
                CombosComprados = c.CombosComprados.Select(cc => new ItemComboCompradoDto
                {
                    NombreCombo = cc.Combo.Nombre, Cantidad = cc.Cantidad, PrecioPagado = cc.PrecioEnElMomento
                }).ToList()
            };
        }
    }
}