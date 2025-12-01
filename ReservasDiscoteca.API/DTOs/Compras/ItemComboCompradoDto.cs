namespace ReservasDiscoteca.API.DTOs.Compras
{
    // DTO NUEVO para el recibo de combos
    public class ItemComboCompradoDto
    {
        public string NombreCombo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioPagado { get; set; }
        
    }
}