using System.ComponentModel.DataAnnotations;

namespace ReservasDiscoteca.API.DTOs.Admin
{
    public class CrearStaffDto
    {
        [Required] 
        public string Nombre { get; set; }
        
        [Required] 
        [EmailAddress] 
        public string Email { get; set; }
        
        [Required] 
        [MinLength(6)] 
        public string Password { get; set; }
        
        [Required]
        public int BolicheId { get; set; } 
    }
}