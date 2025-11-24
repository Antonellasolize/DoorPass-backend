namespace ReservasDiscoteca.API.DTOs.Productos
{
    public class DetalleManillaTipoDto
    {
        public int Id { get; set; } // <--- ID como int
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int BolicheId { get; set; } // <--- ID como int
    }
}