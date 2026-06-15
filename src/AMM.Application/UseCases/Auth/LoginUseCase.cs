using AMM.Application.DTOs.Auth;
using AMM.Application.Interfaces;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Auth;

/// <summary>
/// Valida credenciales y genera un JWT.
/// </summary>
public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUseCase(
        IUsuarioRepository usuarioRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> ExecuteAsync(LoginRequest request, CancellationToken ct = default)
    {
        // 1. Buscar usuario por correo (incluye Roles)
        var usuario = await _usuarioRepository.GetByCorreoAsync(request.Correo, ct);

        if (usuario is null || string.IsNullOrEmpty(usuario.PasswordHash))
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");

        // 2. Verificar contraseña (comparación en tiempo constante)
        if (!_passwordHasher.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos.");

        // 3. Verificar que el usuario esté activo
        if (usuario.EstadoUsuarioId != Domain.Constants.EstadoUsuarioId.Activo)
            throw new UnauthorizedAccessException("El usuario está inactivo. Contacte al administrador.");

        // 4. Obtener roles
        var roles = usuario.Roles.Select(r => r.Nombre).ToList();

        // 5. Generar token y retornar respuesta
        var (token, expira) = _jwtService.GenerateToken(usuario, roles);

        return new LoginResponse(token, expira, usuario.NombreCompleto, usuario.Correo, roles);
    }
}
