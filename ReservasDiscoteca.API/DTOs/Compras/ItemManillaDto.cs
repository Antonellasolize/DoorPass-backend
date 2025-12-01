using System.ComponentModel.DataAnnotations;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // Un item en el "carrito" de manillas
    public class ItemManillaDto
    {
        [Required] public int ManillaTipoId { get; set; } // <--- ID como int
        [Range(1, 10)] public int Cantidad { get; set; }
    }
}