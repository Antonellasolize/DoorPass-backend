using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // DTO NUEVO para el carrito de combos
    public class ItemComboDto
    {
        [Required] public int ComboId { get; set; }
        [Range(1, 10)] public int Cantidad { get; set; }
    }
}