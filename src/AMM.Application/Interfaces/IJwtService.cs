using AMM.Domain.Entities;

namespace AMM.Application.Interfaces;

public interface IJwtService
{
    (string Token, DateTime Expiry) GenerateToken(Usuario usuario, IEnumerable<string> roles);
}
