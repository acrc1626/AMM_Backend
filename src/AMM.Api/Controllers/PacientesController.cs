using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AMM.Application.DTOs;
using AMM.Application.UseCases.Pacientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

/// <summary>
/// Gestión de pacientes — punto de entrada HTTP (Arquitectura Hexagonal)
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class PacientesController : ControllerBase
{
    private readonly CrearPacienteUseCase _crearPacienteUseCase;
    private readonly GetPacienteByIdUseCase _getPacienteByIdUseCase;
    private readonly GetAllPacientesUseCase _getAllPacientesUseCase;
    private readonly UpdatePacienteUseCase _updatePacienteUseCase;
    private readonly ILogger<PacientesController> _logger;

    private string UsuarioActual =>
        User.FindFirstValue(JwtRegisteredClaimNames.Email)
        ?? User.FindFirstValue(ClaimTypes.Email)
        ?? "system";

    public PacientesController(
        CrearPacienteUseCase crearPacienteUseCase,
        GetPacienteByIdUseCase getPacienteByIdUseCase,
        GetAllPacientesUseCase getAllPacientesUseCase,
        UpdatePacienteUseCase updatePacienteUseCase,
        ILogger<PacientesController> logger)
    {
        _crearPacienteUseCase = crearPacienteUseCase;
        _getPacienteByIdUseCase = getPacienteByIdUseCase;
        _getAllPacientesUseCase = getAllPacientesUseCase;
        _updatePacienteUseCase = updatePacienteUseCase;
        _logger = logger;
    }

    /// <summary>Crea un nuevo paciente</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PacienteDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PacienteDto>> Crear(CrearPacienteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _crearPacienteUseCase.ExecuteAsync(request, UsuarioActual, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://amm.ins.gov.co/errors/validation",
                Title = "Error de validación",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext.Request.Path
            });
        }
    }

    /// <summary>Obtiene un paciente por ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PacienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PacienteDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _getPacienteByIdUseCase.ExecuteAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://amm.ins.gov.co/errors/not-found",
                Title = "Paciente no encontrado",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }
        return Ok(result);
    }

    /// <summary>Lista pacientes con paginación</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PacienteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PacienteDto>>> GetAll(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _getAllPacientesUseCase.ExecuteAsync(skip, take, cancellationToken);
        return Ok(result);
    }

    /// <summary>Actualiza un paciente existente</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, UpdatePacienteRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://amm.ins.gov.co/errors/validation",
                Title = "Error de validación",
                Detail = "El ID de la URL no coincide con el cuerpo de la solicitud",
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext.Request.Path
            });
        }

        try
        {
            await _updatePacienteUseCase.ExecuteAsync(request, UsuarioActual, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://amm.ins.gov.co/errors/not-found",
                Title = "Paciente no encontrado",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }
    }
}
