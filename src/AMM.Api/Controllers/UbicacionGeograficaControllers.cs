using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.UbicacionGeografica;
using AMM.Application.UseCases.UbicacionGeografica;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartamentosController : CatalogControllerBase<DepartamentoDto, short>
{
    private readonly DepartamentoUseCase _useCase;

    public DepartamentosController(DepartamentoUseCase useCase, ILogger<DepartamentosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<DepartamentoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllDepartamentosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<DepartamentoDto>> GetById(short id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetDepartamentoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class MunicipiosController : CatalogControllerBase<MunicipioDto, int>
{
    private readonly MunicipioUseCase _useCase;

    public MunicipiosController(MunicipioUseCase useCase, ILogger<MunicipiosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<MunicipioDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllMunicipiosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<MunicipioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetMunicipioByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("departamento/{departamentoId}")]
    public async Task<ActionResult<IReadOnlyList<MunicipioDto>>> GetByDepartamento(short departamentoId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByDepartamentoAsync(departamentoId, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class TerritoriosController : CatalogControllerBase<TerritorioDto, int>
{
    private readonly TerritorioUseCase _useCase;

    public TerritoriosController(TerritorioUseCase useCase, ILogger<TerritoriosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<TerritorioDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllTerritoriosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<TerritorioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetTerritorioByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("municipio/{municipioId}")]
    public async Task<ActionResult<IReadOnlyList<TerritorioDto>>> GetByMunicipio(int municipioId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByMunicipioAsync(municipioId, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class MicroterritoriosController : CatalogControllerBase<MicroterritorioDto, int>
{
    private readonly MicroterritorioUseCase _useCase;

    public MicroterritoriosController(MicroterritorioUseCase useCase, ILogger<MicroterritoriosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<MicroterritorioDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllMicroterritoriosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<MicroterritorioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetMicroterritorioByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("territorio/{territorioId}")]
    public async Task<ActionResult<IReadOnlyList<MicroterritorioDto>>> GetByTerritorio(int territorioId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByTerritorioAsync(territorioId, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class AreasController : CatalogControllerBase<AreaDto, int>
{
    private readonly AreaUseCase _useCase;

    public AreasController(AreaUseCase useCase, ILogger<AreasController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<AreaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAreasAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<AreaDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAreaByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("microterritorio/{microterritorioId}")]
    public async Task<ActionResult<IReadOnlyList<AreaDto>>> GetByMicroterritorio(int microterritorioId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByMicroterritorioAsync(microterritorioId, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class UbicacionesController : CatalogControllerBase<UbicacionDto, long>
{
    private readonly UbicacionUseCase _useCase;

    public UbicacionesController(UbicacionUseCase useCase, ILogger<UbicacionesController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<UbicacionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllUbicacionesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<UbicacionDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetUbicacionByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
