using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReservasDiscoteca.API.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; } // ID numérico secuencial
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

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}