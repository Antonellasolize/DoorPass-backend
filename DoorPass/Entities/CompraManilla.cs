using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDiscoteca.API.Entities
{
    // Tabla de unión para el detalle de la compra
    public class CompraManilla
    {
        [Key]
        public int Id { get; set; } // ID numérico secuencial
        public int Cantidad { get; set; }
        public decimal PrecioEnElMomento { get; set; } // Guarda el precio al que se compró

        [ForeignKey("Compra")]
        public int CompraId { get; set; } // Clave foránea como int
        public Compra Compra { get; set; }

        [ForeignKey("ManillaTipo")]
        public int ManillaTipoId { get; set; } // Clave foránea como int
        public ManillaTipo ManillaTipo { get; set; }
    }
}