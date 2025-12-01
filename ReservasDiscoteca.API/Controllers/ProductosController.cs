using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Productos;

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere estar logueado (cualquier rol)
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductosController(AppDbContext context) { _context = context; }

        // GET /api/productos/boliches
        // (Para ver la lista de todos los boliches)
        [HttpGet("boliches")]
        public async Task<ActionResult<List<DetalleBolicheSimpleDto>>> GetBoliches()
        {
            return await _context.Boliches
                .Select(b => new DetalleBolicheSimpleDto
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Direccion = b.Direccion,
                    Descripcion = b.Descripcion,
                    ImagenUrl = b.ImagenUrl
                })
                .ToListAsync();
        }

        // GET /api/productos/boliches/{bolicheId}
        // (Para ver el detalle de UN boliche, con TODAS sus manillas, mesas y combos disponibles)
        [HttpGet("boliches/{bolicheId}")]
        public async Task<ActionResult<DetalleBolicheDto>> GetBolicheDetalle(int bolicheId)
        {
            var boliche = await _context.Boliches
                .AsNoTracking()
                .Where(b => b.Id == bolicheId)
                .Select(b => new DetalleBolicheDto
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Direccion = b.Direccion,
                    Descripcion = b.Descripcion,
                    ImagenUrl = b.ImagenUrl,
                    // Carga solo manillas con stock
                    Manillas = b.ManillaTipos
                        .Where(m => m.Stock > 0) 
                        .Select(m => new DetalleManillaTipoDto
                        {
                            Id = m.Id, Nombre = m.Nombre, Precio = m.Precio, Stock = m.Stock, BolicheId = m.BolicheId
                        }).ToList(),
                    // Carga solo mesas disponibles
                    Mesas = b.Mesas
                        .Where(m => m.EstaDisponible) 
                        .Select(m => new DetalleMesaDto
                        {
                            Id = m.Id, NombreONumero = m.NombreONumero, Ubicacion = m.Ubicacion, PrecioReserva = m.PrecioReserva, EstaDisponible = m.EstaDisponible, BolicheId = m.BolicheId
                        }).ToList(),
                    // Carga los combos
                    Combos = b.Combos
                        .Select(c => new DetalleComboDto
                        {
                            Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Precio = c.Precio, ImagenUrl = c.ImagenUrl
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (boliche == null) return NotFound("Boliche no encontrado.");
            return Ok(boliche);
        }

        // GET /api/productos/boliches/{bolicheId}/manillas
        // (Para ver SÓLO las manillas de un boliche)
        [HttpGet("boliches/{bolicheId}/manillas")]
        public async Task<ActionResult<List<DetalleManillaTipoDto>>> GetManillasPorBoliche(int bolicheId)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");
            
            return await _context.ManillaTipos
                .Where(m => m.BolicheId == bolicheId && m.Stock > 0)
                .Select(m => new DetalleManillaTipoDto
                {
                    Id = m.Id, Nombre = m.Nombre, Precio = m.Precio, Stock = m.Stock, BolicheId = m.BolicheId
                })
                .ToListAsync();
        }
        
        // GET /api/productos/boliches/{bolicheId}/combos
        // (Para ver SÓLO los combos de un boliche)
        [HttpGet("boliches/{bolicheId}/combos")]
        public async Task<ActionResult<List<DetalleComboDto>>> GetCombosPorBoliche(int bolicheId)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");
            
            return await _context.Combos
                .Where(c => c.BolicheId == bolicheId)
                .Select(c => new DetalleComboDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Precio = c.Precio,
                    ImagenUrl = c.ImagenUrl
                })
                .ToListAsync();
        }

        // --- ¡¡NUEVO ENDPOINT!! ---
        // GET /api/productos/boliches/{bolicheId}/mesas
        // (Para ver SÓLO las mesas disponibles de un boliche)
        [HttpGet("boliches/{bolicheId}/mesas")]
        public async Task<ActionResult<List<DetalleMesaDto>>> GetMesasPorBoliche(int bolicheId)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");

            return await _context.Mesas
                .Where(m => m.BolicheId == bolicheId && m.EstaDisponible) // Filtra solo disponibles
                .Select(m => new DetalleMesaDto
                {
                    Id = m.Id,
                    NombreONumero = m.NombreONumero,
                    Ubicacion = m.Ubicacion,
                    PrecioReserva = m.PrecioReserva,
                    EstaDisponible = m.EstaDisponible,
                    BolicheId = m.BolicheId
                })
                .ToListAsync();
        }
    }
}
