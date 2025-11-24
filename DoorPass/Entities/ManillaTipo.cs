using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDiscoteca.API.Entities
{
    // Las 3 entradas que vende el boliche
    public class ManillaTipo
    {
        [Key]
        public int Id { get; set; } // ID numérico secuencial
        [Required]
        public string Nombre { get; set; } // "General", "VIP", "Consumición"
        public decimal Precio { get; set; }
        public int Stock { get; set; } 

        [ForeignKey("Boliche")]
        public int BolicheId { get; set; } // Clave foránea como int
        public Boliche Boliche { get; set; }
    }
}