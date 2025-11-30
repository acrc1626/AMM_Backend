using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.Censos;
using AMM.Application.UseCases.Censos;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CensosController : CatalogControllerBase<CensoDto, long>
{
    private readonly CensoUseCase _useCase;

    public CensosController(CensoUseCase useCase, ILogger<CensosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<CensoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllCensosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<CensoDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetCensoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("paciente/{pacienteId}")]
    public async Task<ActionResult<IReadOnlyList<CensoDto>>> GetByPaciente(long pacienteId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByPacienteAsync(pacienteId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CensoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CensoDto>> Create(CrearCensoRequest request, CancellationToken cancellationToken)
    {
        var usuario = "system";
        var result = await _useCase.CreateAsync(request, usuario, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, UpdateCensoRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id) return BadRequest("ID mismatch");
        try
        {
            var usuario = "system";
            await _useCase.UpdateAsync(request, usuario, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class CensoNovedadesController : CatalogControllerBase<CensoNovedadDto, long>
{
    private readonly CensoNovedadUseCase _useCase;

    public CensoNovedadesController(CensoNovedadUseCase useCase, ILogger<CensoNovedadesController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllCensoNovedadesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<CensoNovedadDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetCensoNovedadByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("censo/{censoId}")]
    public async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetByCenso(long censoId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByCensoAsync(censoId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("paciente/{pacienteId}")]
    public async Task<ActionResult<IReadOnlyList<CensoNovedadDto>>> GetByPaciente(long pacienteId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByPacienteAsync(pacienteId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CensoNovedadDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CensoNovedadDto>> Create(CrearCensoNovedadRequest request, CancellationToken cancellationToken)
    {
        var usuario = "system";
        var result = await _useCase.CreateAsync(request, usuario, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, UpdateCensoNovedadRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id) return BadRequest("ID mismatch");
        try
        {
            var usuario = "system";
            await _useCase.UpdateAsync(request, usuario, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
