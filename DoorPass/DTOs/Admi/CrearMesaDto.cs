using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Admin
{
    public class CrearMesaDto
    {
        [Required] public string NombreONumero { get; set; }
        public string Ubicacion { get; set; }
        [Range(0, 100)] public decimal PrecioReserva { get; set; }
    }
}