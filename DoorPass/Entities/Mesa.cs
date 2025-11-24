using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDiscoteca.API.Entities
{
    public class Mesa
    {
        [Key]
        public int Id { get; set; } // ID numérico secuencial
        [Required]
        public string NombreONumero { get; set; }
        public string Ubicacion { get; set; } // "Pista Principal", "Balcón VIP"
        public decimal PrecioReserva { get; set; }
        public bool EstaDisponible { get; set; } = true;

        [ForeignKey("Boliche")]
        public int BolicheId { get; set; } // Clave foránea como int
        public Boliche Boliche { get; set; }

        [ForeignKey("Compra")]
        public int? CompraId { get; set; } // Clave foránea opcional (int?)
        public Compra Compra { get; set; }
    }
}