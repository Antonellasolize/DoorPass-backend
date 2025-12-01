using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Compras;
using ReservasDiscoteca.API.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims; // <-- necesario
using System; // <-- necesario para Exception

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Staff,Administrador")]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StaffController(AppDbContext context) { _context = context; }

        // GET /api/staff/historial
        [HttpGet("historial")]
        public async Task<ActionResult> GetHistorialCompras()
        {
            var historialCrudo = await _context.Compras
                .AsNoTracking()
                .Include(c => c.Usuario)
                .Include(c => c.Boliche)
                .Include(c => c.MesaReservada)
                .Include(c => c.ManillasCompradas).ThenInclude(mc => mc.ManillaTipo)
                .Include(c => c.CombosComprados).ThenInclude(cc => cc.Combo)
                .OrderByDescending(h => h.FechaCompra)
                .ToListAsync();

            var historialDto = historialCrudo
                .Select(c => MapToDetalleCompraDto(c))
                .ToList();

            return Ok(historialDto);
        }

        // --- ACCIÓN: INVALIDAR / CONSUMIR UNA COMPRA (STAFF) ---
// PATCH /api/staff/invalidar/5
        [HttpPatch("invalidar/{compraId}")]
        public async Task<IActionResult> InvalidarCompra(int compraId)
        {
            var staffId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Traemos al staff para saber su boliche
            var staff = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == staffId);

            if (staff == null) return Unauthorized("Staff no válido.");

            var esAdmin = User.IsInRole("Administrador");

            // Admin puede invalidar cualquiera
            // Staff solo compras de su boliche
            var compra = await _context.Compras
                .FirstOrDefaultAsync(c =>
                    c.Id == compraId &&
                    (esAdmin || c.BolicheId == staff.BolicheId)
                );

            if (compra == null) 
                return NotFound("Compra no encontrada o no pertenece a tu boliche.");

            if (!compra.EstaActiva) 
                return BadRequest("Esta compra ya fue invalidada.");

            // 1. Marca la compra como inactiva
            // IMPORTANTE: NO devolvemos stock ni mesa porque ya se entregó/usó
            compra.EstaActiva = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Compra invalidada correctamente." });
        }
        // --- MÉTODO HELPER PRIVADO ---
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
                    ? $"{c.MesaReservada.NombreONumero} - {c.MesaReservada.Ubicacion}"
                    : null,
                ManillasCompradas = c.ManillasCompradas.Select(mc => new ItemCompradoDto
                {
                    NombreManilla = mc.ManillaTipo.Nombre,
                    Cantidad = mc.Cantidad,
                    PrecioPagado = mc.PrecioEnElMomento
                }).ToList(),
                CombosComprados = c.CombosComprados.Select(cc => new ItemComboCompradoDto
                {
                    NombreCombo = cc.Combo.Nombre,
                    Cantidad = cc.Cantidad,
                    PrecioPagado = cc.PrecioEnElMomento
                }).ToList()
            };
        }
    }
}