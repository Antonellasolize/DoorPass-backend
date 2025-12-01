namespace ReservasDiscoteca.API.DTOs.Compras
{
    // Helper DTO para el recibo
    public class ItemCompradoDto
    {
        public string NombreManilla { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioPagado { get; set; }
    }
}