using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Admin
{
    // DTO para ACTUALIZAR.
    public class UpdateBolicheDto
    {
        [Required] public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; } // Opcional
    }
}