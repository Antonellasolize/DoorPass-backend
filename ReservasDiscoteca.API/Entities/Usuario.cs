using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ReservasDiscoteca.API.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string Rol { get; set; } // "Admin", "Staff", "Usuario"
        
        [ForeignKey("Boliche")]
        public int? BolicheId { get; set; } 
        public Boliche Boliche { get; set; }

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}