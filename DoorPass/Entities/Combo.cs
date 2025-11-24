using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDiscoteca.API.Entities
{
    // La nueva entidad para Combos de bebidas
    public class Combo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        
        [Range(0, 10000)]
        public decimal Precio { get; set; }
        
        public string ImagenUrl { get; set; } // Opcional (link a la imagen)

        [ForeignKey("Boliche")]
        public int BolicheId { get; set; }
        public Boliche Boliche { get; set; }
    }
}