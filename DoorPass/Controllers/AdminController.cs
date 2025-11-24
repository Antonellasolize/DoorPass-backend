using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Admin;
using ReservasDiscoteca.API.DTOs.Auth;
using ReservasDiscoteca.API.Entities;
using System.Security.Cryptography;
using System.Text;

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] // ¡SOLO ADMINS!
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context) { _context = context; }

        // --- GESTIÓN DE BOLICHES (CRUD) ---
        [HttpPost("boliches")]
        public async Task<IActionResult> CrearBoliche(CrearBolicheDto bolicheDto)
        {
            var boliche = new Boliche
            {
                Nombre = bolicheDto.Nombre,
                Direccion = bolicheDto.Direccion,
                Descripcion = bolicheDto.Descripcion,
                ImagenUrl = bolicheDto.ImagenUrl
            };
            _context.Boliches.Add(boliche);
            await _context.SaveChangesAsync();
            return Ok(boliche);
        }

        [HttpPut("boliches/{id}")]
        public async Task<IActionResult> UpdateBoliche(int id, UpdateBolicheDto bolicheDto)
        {
            var boliche = await _context.Boliches.FindAsync(id);
            if (boliche == null) return NotFound("Boliche no encontrado.");

            boliche.Nombre = bolicheDto.Nombre;
            boliche.Direccion = bolicheDto.Direccion;
            boliche.Descripcion = bolicheDto.Descripcion;
            boliche.ImagenUrl = bolicheDto.ImagenUrl;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("boliches/{id}")]
        public async Task<IActionResult> DeleteBoliche(int id)
        {
            var boliche = await _context.Boliches.FindAsync(id);
            if (boliche == null) return NotFound("Boliche no encontrado.");

            _context.Boliches.Remove(boliche); // Borrado en cascada (Cascade)
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- GESTIÓN DE MANILLAS (CRUD) ---
        [HttpPost("boliches/{bolicheId}/manillas")]
        public async Task<IActionResult> CrearManilla(int bolicheId, CrearManillaTipoDto manillaDto)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");

            var manillaTipo = new ManillaTipo
            {
                Nombre = manillaDto.Nombre,
                Precio = manillaDto.Precio,
                Stock = manillaDto.Stock,
                BolicheId = bolicheId
            };
            _context.ManillaTipos.Add(manillaTipo);
            await _context.SaveChangesAsync();
            return Ok(manillaTipo);
        }

        [HttpPut("manillas/{id}")]
        public async Task<IActionResult> UpdateManilla(int id, UpdateManillaTipoDto manillaDto)
        {
            var manilla = await _context.ManillaTipos.FindAsync(id);
            if (manilla == null) return NotFound("Manilla no encontrada.");

            manilla.Nombre = manillaDto.Nombre;
            manilla.Precio = manillaDto.Precio;
            manilla.Stock = manillaDto.Stock;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("manillas/{id}")]
        public async Task<IActionResult> DeleteManilla(int id)
        {
            var manilla = await _context.ManillaTipos.FindAsync(id);
            if (manilla == null) return NotFound("Manilla no encontrada.");
            _context.ManillaTipos.Remove(manilla);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- GESTIÓN DE MESAS (CRUD) ---
        [HttpPost("boliches/{bolicheId}/mesas")]
        public async Task<IActionResult> CrearMesa(int bolicheId, CrearMesaDto mesaDto)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");

            var mesa = new Mesa
            {
                NombreONumero = mesaDto.NombreONumero,
                Ubicacion = mesaDto.Ubicacion,
                PrecioReserva = mesaDto.PrecioReserva,
                EstaDisponible = true,
                BolicheId = bolicheId
            };
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();
            return Ok(mesa);
        }
        
        [HttpPut("mesas/{id}")]
        public async Task<IActionResult> UpdateMesa(int id, UpdateMesaDto mesaDto)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null) return NotFound("Mesa no encontrada.");

            mesa.NombreONumero = mesaDto.NombreONumero;
            mesa.Ubicacion = mesaDto.Ubicacion;
            mesa.PrecioReserva = mesaDto.PrecioReserva;
            mesa.EstaDisponible = mesaDto.EstaDisponible; // Permite re-habilitarla
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("mesas/{id}")]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null) return NotFound("Mesa no encontrada.");
            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- GESTIÓN DE COMBOS (CRUD) ---
        [HttpPost("boliches/{bolicheId}/combos")]
        public async Task<IActionResult> CrearCombo(int bolicheId, CrearComboDto comboDto)
        {
            if (!await _context.Boliches.AnyAsync(b => b.Id == bolicheId))
                return NotFound("Boliche no encontrado.");

            var combo = new Combo
            {
                Nombre = comboDto.Nombre,
                Descripcion = comboDto.Descripcion,
                Precio = comboDto.Precio,
                ImagenUrl = comboDto.ImagenUrl,
                BolicheId = bolicheId
            };
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
            return Ok(combo);
        }

        [HttpPut("combos/{id}")]
        public async Task<IActionResult> UpdateCombo(int id, UpdateComboDto comboDto)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null) return NotFound("Combo no encontrado.");

            combo.Nombre = comboDto.Nombre;
            combo.Descripcion = comboDto.Descripcion;
            combo.Precio = comboDto.Precio;
            combo.ImagenUrl = comboDto.ImagenUrl;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("combos/{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null) return NotFound("Combo no encontrado.");
            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- GESTIÓN DE STAFF ---
        [HttpPost("crear-staff")]
        public async Task<ActionResult<UsuarioDto>> CreateStaff(CrearStaffDto staffDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == staffDto.Email.ToLower()))
                return BadRequest("El email ya está en uso.");

            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                Nombre = staffDto.Nombre,
                Email = staffDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(staffDto.Password)),
                PasswordSalt = hmac.Key,
                Rol = "Staff"
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new UsuarioDto { Id = usuario.Id, Nombre = usuario.Nombre, Email = usuario.Email, Rol = usuario.Rol, Token = "" });
        }
    }
}