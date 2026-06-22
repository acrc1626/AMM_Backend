using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.Censos;
using AMM.Application.DTOs.Common;
using AMM.Application.UseCases.Censos;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

/// <summary>
/// Controlador de censos.
/// POST  /api/Censos          — HU-001 (Hogar), HU-002 (Educativo), HU-003 (Institucional)
/// GET   /api/Censos          — listado liviano paginable
/// GET   /api/Censos/{id}     — HU-004: detalle completo con personas y enfermedades
/// PUT   /api/Censos/{id}     — HU-005: editar campos del censo sin afectar personas
/// PATCH /api/Censos/{id}     — DEF-AMM-004: agregar personas, tratamientos y novedad censal
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CensosController : CatalogControllerBase<CensoDto, long>
{
    private readonly CensoUseCase         _censoUseCase;
    private readonly CrearCensoUseCase    _crearUseCase;
    private readonly ConsultarCensoUseCase _consultarUseCase;

    /// <summary>Inicializa el controlador con los casos de uso de censos.</summary>
    public CensosController(
        CensoUseCase          censoUseCase,
        CrearCensoUseCase     crearUseCase,
        ConsultarCensoUseCase consultarUseCase,
        ILogger<CensosController> logger) : base(logger)
    {
        this._censoUseCase     = censoUseCase;
        this._crearUseCase     = crearUseCase;
        this._consultarUseCase = consultarUseCase;
    }

    // ─── GET all ─────────────────────────────────────────────────────────────

    /// <summary>Retorna todos los censos como proyección liviana.</summary>
    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<CensoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await this._censoUseCase.GetAllCensosAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna censos paginados.</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<CensoDto>>> GetAllPaged(
        [FromQuery] int page     = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        pageSize = Math.Min(pageSize, 100);
        var result = await this._censoUseCase.GetAllCensosPagedAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    // ─── GET by ID (HU-004) ──────────────────────────────────────────────────

    /// <summary>
    /// Retorna el detalle completo del censo: caracterización, personas,
    /// parentescos, estados y enfermedades resueltos (HU-004).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CensoDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public override async Task<ActionResult<CensoDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await this._consultarUseCase.GetDetalleAsync(id, cancellationToken);
        if (result is null) return HandleResult<CensoDto>(null);
        return Ok(result);
    }

    // ─── POST (HU-001, HU-002, HU-003) ──────────────────────────────────────

    /// <summary>
    /// Crea un censo con el wizard completo.
    /// El tipo de entorno (TipoEntornoId) determina las reglas aplicadas:
    /// Hogar requiere JefeHogar; Educativo e Institucional no lo admiten.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CensoResumenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CensoResumenDto>> Create(
        [FromBody] CrearCensoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await this._crearUseCase.CrearAsync(request, UsuarioActual, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title  = "Validación fallida",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    // ─── PUT (HU-005) ────────────────────────────────────────────────────────

    /// <summary>
    /// Actualiza los campos editables del censo sin afectar las personas asociadas.
    /// RN-011: conserva el Jefe de Hogar. RN-012: conserva la integridad de personas.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateCensoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await this._censoUseCase.UpdateAsync(request with { Id = id }, UsuarioActual, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // ─── PATCH (DEF-AMM-004) ─────────────────────────────────────────────────

    /// <summary>
    /// Agrega personas, tratamientos y novedad censal a un censo ya creado.
    /// No modifica la caracterización del entorno guardada en el POST inicial.
    /// </summary>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AgregarPersonasYTratamientos(
        long id,
        [FromBody] AgregarPersonasYTratamientosRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await this._crearUseCase.AgregarPersonasYTratamientosAsync(
                id, request, UsuarioActual, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title  = "Validación fallida",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}

/// <summary>Controlador de novedades de censo.</summary>
[ApiController]
[Route("api/[controller]")]
public class CensoNovedadesController : CatalogControllerBase<CensoNovedadDto, long>
{
    private readonly CensoNovedadUseCase _useCase;

    /// <summary>Inicializa el controlador con el caso de uso de novedades.</summary>
    public CensoNovedadesController(CensoNovedadUseCase useCase, ILogger<CensoNovedadesController> logger)
        : base(logger)
    {
        this._useCase = useCase;
    }

    /// <summary>Retorna todas las novedades de censo.</summary>
    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await this._useCase.GetAllCensoNovedadesAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna una novedad por ID.</summary>
    [HttpGet("{id}")]
    public override async Task<ActionResult<CensoNovedadDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await this._useCase.GetCensoNovedadByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>Retorna las novedades de un censo específico.</summary>
    [HttpGet("censo/{censoId}")]
    public async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetByCenso(long censoId, CancellationToken cancellationToken)
    {
        var result = await this._useCase.GetByCensoAsync(censoId, cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna las novedades asociadas a un paciente.</summary>
    [HttpGet("paciente/{pacienteId}")]
    public async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetByPaciente(long pacienteId, CancellationToken cancellationToken)
    {
        var result = await this._useCase.GetByPacienteAsync(pacienteId, cancellationToken);
        return Ok(result);
    }

    /// <summary>Crea una novedad de censo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CensoNovedadDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CensoNovedadDto>> Create(CrearCensoNovedadRequest request, CancellationToken cancellationToken)
    {
        var result = await this._useCase.CreateAsync(request, UsuarioActual, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Actualiza una novedad de censo.</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, UpdateCensoNovedadRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await this._useCase.UpdateAsync(request with { Id = id }, UsuarioActual, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
