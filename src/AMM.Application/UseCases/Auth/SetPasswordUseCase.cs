using AMM.Application.DTOs.Auth;
using AMM.Application.Interfaces;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Auth;

/// <summary>
/// Asigna o cambia la contraseña de un usuario.
/// </summary>
public class SetPasswordUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public SetPasswordUseCase(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    /// <summary>Administrador asigna contraseña inicial a un usuario.</summary>
    public async Task ExecuteAsync(SetPasswordRequest request, string operador, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId, ct)
            ?? throw new KeyNotFoundException($"Usuario con ID {request.UsuarioId} no encontrado.");

        usuario.PasswordHash = _passwordHasher.Hash(request.Password);
        usuario.ModificadoEn = DateTime.UtcNow;
        usuario.ModificadoPor = operador;

        await _usuarioRepository.UpdateAsync(usuario, ct);
        await _usuarioRepository.SaveChangesAsync(ct);
    }

    /// <summary>El propio usuario cambia su contraseña (valida la actual).</summary>
    public async Task ChangeAsync(int usuarioId, ChangePasswordRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, ct)
            ?? throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        if (string.IsNullOrEmpty(usuario.PasswordHash) ||
            !_passwordHasher.Verify(request.PasswordActual, usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException("La contraseña actual es incorrecta.");
        }

        usuario.PasswordHash = _passwordHasher.Hash(request.PasswordNuevo);
        usuario.ModificadoEn = DateTime.UtcNow;
        usuario.ModificadoPor = usuario.Correo;

        await _usuarioRepository.UpdateAsync(usuario, ct);
        await _usuarioRepository.SaveChangesAsync(ct);
    }
}
