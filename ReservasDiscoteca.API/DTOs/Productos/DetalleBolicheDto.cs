using System.Collections.Generic;
namespace ReservasDiscoteca.API.DTOs.Productos
{
    // ACTUALIZADO con ImagenUrl, Descripcion y Combos
    public class DetalleBolicheDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        
        public List<DetalleManillaTipoDto> Manillas { get; set; } = new List<DetalleManillaTipoDto>();
        public List<DetalleMesaDto> Mesas { get; set; } = new List<DetalleMesaDto>();
        public List<DetalleComboDto> Combos { get; set; } = new List<DetalleComboDto>(); // <-- NUEVO
    }
}