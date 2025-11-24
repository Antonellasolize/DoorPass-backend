using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReservasDiscoteca.API.Entities
{
    public class Boliche
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        // --- ATRIBUTOS NUEVOS ---
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; } // Opcional (link a la imagen)
        // ------------------------

        public ICollection<ManillaTipo> ManillaTipos { get; set; } = new List<ManillaTipo>();
        public ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
        
        // --- RELACIÓN NUEVA ---
        public ICollection<Combo> Combos { get; set; } = new List<Combo>();
        // ----------------------
    }
}