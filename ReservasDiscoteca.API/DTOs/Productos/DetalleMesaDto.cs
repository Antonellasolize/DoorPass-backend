namespace ReservasDiscoteca.API.DTOs.Productos
{
    public class DetalleMesaDto
    {
        public int Id { get; set; } // <--- ID como int
        public string NombreONumero { get; set; }
        public string Ubicacion { get; set; }
        public decimal PrecioReserva { get; set; }
        public bool EstaDisponible { get; set; }
        public int BolicheId { get; set; } // <--- ID como int
    }
}