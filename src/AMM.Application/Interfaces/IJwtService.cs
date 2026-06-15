using AMM.Domain.Entities;

namespace AMM.Application.Interfaces;

/// <summary>
/// Puerto de dominio para la generación de tokens JWT.
/// La implementación vive en Infrastructure.
/// </summary>
public interface IJwtService
{
    (string Token, DateTime Expiry) GenerateToken(Usuario usuario, IEnumerable<string> roles);
}
