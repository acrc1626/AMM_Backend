using System.ComponentModel.DataAnnotations;

namespace AMM.Application.DTOs.Auth;

/// <summary>Credenciales de acceso</summary>
public record LoginRequest(
    [Required, EmailAddress] string Correo,
    [Required, MinLength(6)] string Password
);

/// <summary>Respuesta exitosa de autenticación</summary>
public record LoginResponse(
    string Token,
    DateTime Expira,
    string NombreCompleto,
    string Correo,
    IEnumerable<string> Roles
);

/// <summary>Cambio de contraseña por el usuario autenticado</summary>
public record ChangePasswordRequest(
    [Required, MinLength(6)] string PasswordActual,
    [Required, MinLength(6)] string PasswordNuevo
);

/// <summary>Asignación de contraseña inicial por administrador</summary>
public record SetPasswordRequest(
    int UsuarioId,
    [Required, MinLength(6)] string Password
);
