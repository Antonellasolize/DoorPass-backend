using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDiscoteca.API.Entities
{
    public class CompraCombo
    {
        [Key]
        public int Id { get; set; }
        
        public int Cantidad { get; set; }
        
        public decimal PrecioEnElMomento { get; set; }

        [ForeignKey("Compra")]
        public int CompraId { get; set; }
        public Compra Compra { get; set; }

        [ForeignKey("Combo")]
        public int ComboId { get; set; }
        public Combo Combo { get; set; }
    }
}