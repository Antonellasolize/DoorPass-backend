using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Admin
{
    public class UpdateMesaDto
    {
        [Required] public string NombreONumero { get; set; }
        public string Ubicacion { get; set; }
        [Range(0, 10000)] public decimal PrecioReserva { get; set; }
        public bool EstaDisponible { get; set; } // El Admin puede re-habilitar una mesa
    }
}