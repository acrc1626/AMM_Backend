using System.Security.Claims;
using AMM.Application.DTOs.Auth;
using AMM.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Autenticación")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly SetPasswordUseCase _setPasswordUseCase;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        LoginUseCase loginUseCase,
        SetPasswordUseCase setPasswordUseCase,
        ILogger<AuthController> logger)
    {
        _loginUseCase = loginUseCase;
        _setPasswordUseCase = setPasswordUseCase;
        _logger = logger;
    }

    /// <summary>Inicia sesión y obtiene un token JWT</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _loginUseCase.ExecuteAsync(request, cancellationToken);
            _logger.LogInformation("Login exitoso: {Correo}", request.Correo);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Intento de login fallido: {Correo} — {Mensaje}", request.Correo, ex.Message);
            return Unauthorized(new ProblemDetails
            {
                Title = "Credenciales inválidas",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    /// <summary>Cambia la contraseña del usuario autenticado</summary>
    [HttpPut("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Claim sub no encontrado."));

        try
        {
            await _setPasswordUseCase.ChangeAsync(usuarioId, request, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Contraseña incorrecta",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    /// <summary>
    /// Asigna contraseña inicial a un usuario (solo administradores).
    /// Usar en la primera configuración del sistema.
    /// </summary>
    [HttpPost("set-password")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetPassword(
        [FromBody] SetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var operador = User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)
            ?? User.FindFirstValue(ClaimTypes.Email)
            ?? "system";

        try
        {
            await _setPasswordUseCase.ExecuteAsync(request, operador, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>Devuelve la información del usuario autenticado actualmente</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new
        {
            Sub = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Email = User.FindFirstValue(ClaimTypes.Email),
            Name = User.FindFirstValue(ClaimTypes.Name),
            Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
        });
    }
}
