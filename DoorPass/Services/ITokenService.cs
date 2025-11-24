using ReservasDiscoteca.API.Entities;

namespace ReservasDiscoteca.API.Services
{
    public interface ITokenService
    {
        string CreateToken(Usuario usuario);
    }
}