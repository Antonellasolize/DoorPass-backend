using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Auth
{
    public class RegistroDto
    {
        [Required] public string Nombre { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] [MinLength(6)] public string Password { get; set; }
    }
}