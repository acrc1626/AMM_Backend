using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Eventos;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EscabiosisController : CatalogControllerBase<EscabiosisDto, long>
{
    private readonly EscabiosisUseCase _useCase;

    public EscabiosisController(EscabiosisUseCase useCase, ILogger<EscabiosisController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<EscabiosisDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<EscabiosisDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class GeohelmintiasisController : CatalogControllerBase<GeohelmintiasisDto, long>
{
    private readonly GeohelmintiasisUseCase _useCase;

    public GeohelmintiasisController(GeohelmintiasisUseCase useCase, ILogger<GeohelmintiasisController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<GeohelmintiasisDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<GeohelmintiasisDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class PediculosisController : CatalogControllerBase<PediculosisDto, long>
{
    private readonly PediculosisUseCase _useCase;

    public PediculosisController(PediculosisUseCase useCase, ILogger<PediculosisController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<PediculosisDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<PediculosisDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class MalariaController : CatalogControllerBase<MalariaDto, long>
{
    private readonly MalariaUseCase _useCase;

    public MalariaController(MalariaUseCase useCase, ILogger<MalariaController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<MalariaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<MalariaDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class TuberculosisController : CatalogControllerBase<TuberculosisDto, long>
{
    private readonly TuberculosisUseCase _useCase;

    public TuberculosisController(TuberculosisUseCase useCase, ILogger<TuberculosisController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<TuberculosisDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<TuberculosisDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class TuberculosisContactoController : CatalogControllerBase<TuberculosisContactoDto, long>
{
    private readonly TuberculosisContactoUseCase _useCase;

    public TuberculosisContactoController(TuberculosisContactoUseCase useCase, ILogger<TuberculosisContactoController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<TuberculosisContactoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<TuberculosisContactoDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class LeshmaniasisCutaneaController : CatalogControllerBase<LeshmaniasisCutaneaDto, long>
{
    private readonly LeshmaniasisCutaneaUseCase _useCase;

    public LeshmaniasisCutaneaController(LeshmaniasisCutaneaUseCase useCase, ILogger<LeshmaniasisCutaneaController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<LeshmaniasisCutaneaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<LeshmaniasisCutaneaDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
