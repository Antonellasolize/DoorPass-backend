using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Compras;
using ReservasDiscoteca.API.Entities; // Necesario para el Map
using System.Linq;
using System.Threading.Tasks;

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Staff,Administrador")] // Staff y Admin pueden ver
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StaffController(AppDbContext context) { _context = context; }

        // GET /api/staff/historial
        [HttpGet("historial")]
        public async Task<ActionResult> GetHistorialCompras()
        {
            // --- ESTA ES LA PARTE CORREGIDA ---

            // 1. Traer los datos "crudos" de la BD a la memoria
            //    (Incluyendo los Includes y el OrderBy)
            var historialCrudo = await _context.Compras
                .AsNoTracking()
                .Include(c => c.Usuario) 
                .Include(c => c.Boliche) 
                .Include(c => c.MesaReservada) 
                .Include(c => c.ManillasCompradas).ThenInclude(mc => mc.ManillaTipo)
                .Include(c => c.CombosComprados).ThenInclude(cc => cc.Combo)
                .OrderByDescending(h => h.FechaCompra) // Ordenamos en la BD (eficiente)
                .ToListAsync(); // <-- ¡Traemos a memoria!

            // 2. Ahora que están en memoria, los mapeamos usando C#
            var historialDto = historialCrudo
                .Select(c => MapToDetalleCompraDto(c)) 
                .ToList();
            
            return Ok(historialDto);
            // ------------------------------------
        }

        // --- MÉTODO HELPER PRIVADO (Este estaba bien) ---
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