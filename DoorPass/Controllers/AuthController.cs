using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Data;
using ReservasDiscoteca.API.DTOs.Auth;
using ReservasDiscoteca.API.Entities;
using ReservasDiscoteca.API.Services;
using System.Security.Cryptography;
using System.Text;

namespace ReservasDiscoteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> Register(RegistroDto registroDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registroDto.Email.ToLower()))
                return BadRequest("El email ya está en uso.");

            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                Nombre = registroDto.Nombre,
                Email = registroDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroDto.Password)),
                PasswordSalt = hmac.Key,
                Rol = "Usuario" // Rol por defecto
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = _tokenService.CreateToken(usuario)
            };
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .SingleOrDefaultAsync(u => u.Email == loginDto.Email.ToLower());

            if (usuario == null) return Unauthorized("Email o contraseña inválida.");

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i])
                    return Unauthorized("Email o contraseña inválida.");
            }
            
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = _tokenService.CreateToken(usuario)
            };
        }
    }
}