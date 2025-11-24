using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // DTO para la Acción 2: Reservar Mesa
    public class ReservarMesaDto
    {
        [Required] public int BolicheId { get; set; } // <--- ID como int
        [Required] public int MesaId { get; set; } // <--- ID como int
    }
}