namespace ReservasDiscoteca.API.DTOs.Productos
{
    // DTO NUEVO para mostrar un Combo
    public class DetalleComboDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
    }
}