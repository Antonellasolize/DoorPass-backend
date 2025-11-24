using System;
using System.Collections.Generic;
namespace ReservasDiscoteca.API.DTOs.Compras
{
    // ACTUALIZADO con EstaActiva y la lista de combos
    public class DetalleCompraDto
    {
        public int CompraId { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal TotalPagado { get; set; }
        public string TipoCompra { get; set; } // "Manillas", "Reserva de Mesa" o "Combos"
        public bool EstaActiva { get; set; } // <-- NUEVO
        
        public int BolicheId { get; set; }
        public string NombreBoliche { get; set; }
        
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string EmailUsuario { get; set; }
        
        public string MesaReservada { get; set; }
        
        public List<ItemCompradoDto> ManillasCompradas { get; set; } = new List<ItemCompradoDto>();
        public List<ItemComboCompradoDto> CombosComprados { get; set; } = new List<ItemComboCompradoDto>(); // <-- NUEVO
    }
}