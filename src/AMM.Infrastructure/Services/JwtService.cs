using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AMM.Application.Interfaces;
using AMM.Application.Settings;
using AMM.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AMM.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, DateTime Expiry) GenerateToken(Usuario usuario, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var expiry = now.AddMinutes(_settings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new(JwtRegisteredClaimNames.Name,  usuario.NombreCompleto),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

        foreach (var rol in roles)
            claims.Add(new Claim(ClaimTypes.Role, rol));

        var token = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            notBefore:          now,
            expires:            expiry,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiry);
    }
}
