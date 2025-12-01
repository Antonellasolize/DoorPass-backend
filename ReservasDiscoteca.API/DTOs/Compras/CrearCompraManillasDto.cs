using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // DTO para la Acción 1: Comprar Manillas
    public class CrearCompraManillasDto
    {
        [Required] public int BolicheId { get; set; } // <--- ID como int
        [Required] public List<ItemManillaDto> Manillas { get; set; } = new List<ItemManillaDto>();
    }
}