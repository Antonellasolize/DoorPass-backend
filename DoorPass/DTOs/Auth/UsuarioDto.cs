namespace ReservasDiscoteca.API.DTOs.Auth
{
    public class UsuarioDto
    {
        public int Id { get; set; } // <--- ID como int
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
    }
}