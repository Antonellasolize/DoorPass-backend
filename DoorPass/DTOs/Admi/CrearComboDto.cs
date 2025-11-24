using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Admin
{
    // DTO NUEVO para crear Combos
    public class CrearComboDto
    {
        [Required] public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Range(0, 10000)] public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } // Opcional
    }
}