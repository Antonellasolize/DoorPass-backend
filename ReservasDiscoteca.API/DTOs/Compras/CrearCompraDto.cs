using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReservasDiscoteca.API.DTOs.Compras
{
    // El "carrito" que envía el usuario
    public class CrearCompraDto
    {
        [Required]
        public Guid BolicheId { get; set; }
        
        public List<ItemManillaDto> Manillas { get; set; } = new List<ItemManillaDto>();
        
        // El ID de la mesa que quieren reservar (opcional)
        public Guid? MesaId { get; set; } 
    }
}