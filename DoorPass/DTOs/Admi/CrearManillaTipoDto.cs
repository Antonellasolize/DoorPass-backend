using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Admin
{
    public class CrearManillaTipoDto
    {
        [Required] public string Nombre { get; set; }
        [Range(0, 5000)] public decimal Precio { get; set; }
        [Range(0, 1000)] public int Stock { get; set; }
    }
}