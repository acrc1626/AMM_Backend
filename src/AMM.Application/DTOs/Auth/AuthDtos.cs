using System.ComponentModel.DataAnnotations;

namespace AMM.Application.DTOs.Auth;

public record LoginRequest(
    [Required, EmailAddress] string Correo,
    [Required, MinLength(6)] string Password
);

public record LoginResponse(
    string Token,
    DateTime Expira,
    string NombreCompleto,
    string Correo,
    IEnumerable<string> Roles
);

public record ChangePasswordRequest(
    [Required, MinLength(6)] string PasswordActual,
    [Required, MinLength(6)] string PasswordNuevo
);

public record SetPasswordRequest(
    int UsuarioId,
    [Required, MinLength(6)] string Password
);
