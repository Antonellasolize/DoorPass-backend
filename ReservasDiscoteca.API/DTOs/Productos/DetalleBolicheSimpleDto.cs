namespace ReservasDiscoteca.API.DTOs.Productos
{
    // ACTUALIZADO con ImagenUrl y Descripcion
    public class DetalleBolicheSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
    }
}