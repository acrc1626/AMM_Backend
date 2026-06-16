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
    private readonly ILogger<AuthController> _logger;

    public AuthController(LoginUseCase loginUseCase, ILogger<AuthController> logger)
    {
        _loginUseCase = loginUseCase;
        _logger = logger;
    }

    /// <summary>Valida credenciales contra la BD y retorna un JWT.</summary>
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
            _logger.LogWarning("Login fallido: {Correo}", request.Correo);
            return Unauthorized(new ProblemDetails
            {
                Title = "Credenciales inválidas",
                Detail = ex.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }
}
