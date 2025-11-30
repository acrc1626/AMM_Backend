using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.Catalogos;
using AMM.Application.UseCases.Catalogos;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

/// <summary>
/// Estados catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EstadosController : CatalogControllerBase<EstadoDto, byte>
{
    private readonly EstadoUseCase _useCase;

    public EstadosController(EstadoUseCase useCase, ILogger<EstadosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<EstadoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllEstadosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<EstadoDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetEstadoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Tipos de Documento catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TiposDocumentoController : CatalogControllerBase<TipoDocumentoDto, byte>
{
    private readonly TipoDocumentoUseCase _useCase;

    public TiposDocumentoController(TipoDocumentoUseCase useCase, ILogger<TiposDocumentoController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<TipoDocumentoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllTiposDocumentoAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<TipoDocumentoDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetTipoDocumentoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Sexos catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SexosController : CatalogControllerBase<SexoDto, byte>
{
    private readonly SexoUseCase _useCase;

    public SexosController(SexoUseCase useCase, ILogger<SexosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<SexoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllSexosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<SexoDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetSexoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Etnias catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EtniasController : CatalogControllerBase<EtniaDto, byte>
{
    private readonly EtniaUseCase _useCase;

    public EtniasController(EtniaUseCase useCase, ILogger<EtniasController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<EtniaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllEtniasAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<EtniaDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetEtniaByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Pueblos Indígenas catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PueblosIndigenasController : CatalogControllerBase<PuebloIndigenaDto, short>
{
    private readonly PuebloIndigenaUseCase _useCase;

    public PueblosIndigenasController(PuebloIndigenaUseCase useCase, ILogger<PueblosIndigenasController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<PuebloIndigenaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllPueblosIndigenasAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<PuebloIndigenaDto>> GetById(short id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetPuebloIndigenaByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Tipos de Entorno catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TiposEntornoController : CatalogControllerBase<TipoEntornoDto, byte>
{
    private readonly TipoEntornoUseCase _useCase;

    public TiposEntornoController(TipoEntornoUseCase useCase, ILogger<TiposEntornoController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<TipoEntornoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllTiposEntornoAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<TipoEntornoDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetTipoEntornoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Tipos de Evento catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventoTiposController : CatalogControllerBase<EventoTipoDto, byte>
{
    private readonly EventoTipoUseCase _useCase;

    public EventoTiposController(EventoTipoUseCase useCase, ILogger<EventoTiposController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<EventoTipoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllEventoTiposAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<EventoTipoDto>> GetById(byte id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetEventoTipoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

/// <summary>
/// Formas Farmacéuticas catalog endpoint
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FormasFarmaceuticasController : CatalogControllerBase<FormaFarmaceuticaDto, short>
{
    private readonly FormaFarmaceuticaUseCase _useCase;

    public FormasFarmaceuticasController(FormaFarmaceuticaUseCase useCase, ILogger<FormasFarmaceuticasController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<FormaFarmaceuticaDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllFormasFarmaceuticasAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<FormaFarmaceuticaDto>> GetById(short id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetFormaFarmaceuticaByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
