using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // DTO NUEVO para la acción de comprar combos
    public class CrearCompraCombosDto
    {
        [Required] public int BolicheId { get; set; }
        [Required] public List<ItemComboDto> Combos { get; set; } = new List<ItemComboDto>();
    }
}