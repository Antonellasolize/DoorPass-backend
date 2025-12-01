namespace ReservasDiscoteca.API.DTOs.Auth
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public int? BolicheId { get; set; } // <-- NUEVO
        public string Token { get; set; }
    }
}