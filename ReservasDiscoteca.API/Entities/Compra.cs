using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ReservasDiscoteca.API.Entities
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
        public decimal TotalPagado { get; set; }
        
        [Required]
        public string TipoCompra { get; set; } // "Manillas", "Reserva de Mesa" o "Combos"

        // --- ATRIBUTO NUEVO ---
        // Para que el usuario pueda cancelar
        public bool EstaActiva { get; set; } = true; 
        // --------------------

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [ForeignKey("Boliche")]
        public int BolicheId { get; set; }
        public Boliche Boliche { get; set; }

        // Si la compra fue de manillas
        public ICollection<CompraManilla> ManillasCompradas { get; set; } = new List<CompraManilla>();
        
        // --- RELACIÓN NUEVA ---
        // Si la compra fue de combos
        public ICollection<CompraCombo> CombosComprados { get; set; } = new List<CompraCombo>();
        // --------------------
        
        // Si la compra fue de una mesa
        public Mesa MesaReservada { get; set; }
    }
}