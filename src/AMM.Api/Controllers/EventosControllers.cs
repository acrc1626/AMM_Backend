using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Eventos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/escabiosis")]
public class EscabiosisController : ControllerBase
{
    private readonly EventoEscabiosisUseCase _useCase;

    public EscabiosisController(EventoEscabiosisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/geohelmintiasis")]
public class GeohelmintiasisController : ControllerBase
{
    private readonly EventoGeohelmintiasisUseCase _useCase;

    public GeohelmintiasisController(EventoGeohelmintiasisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/pediculosis")]
public class PediculosisController : ControllerBase
{
    private readonly EventoPediculosisUseCase _useCase;

    public PediculosisController(EventoPediculosisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/pian")]
public class PianController : ControllerBase
{
    private readonly EventoPianUseCase _useCase;

    public PianController(EventoPianUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/teniasiscisticercosis")]
public class TeniasisCisticercosisController : ControllerBase
{
    private readonly EventoTeniasisCisticercosisUseCase _useCase;

    public TeniasisCisticercosisController(EventoTeniasisCisticercosisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/tracoma")]
public class TracomaController : ControllerBase
{
    private readonly EventoTracomaUseCase _useCase;

    public TracomaController(EventoTracomaUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/tungiasis")]
public class TungiasisController : ControllerBase
{
    private readonly EventoTungiasisUseCase _useCase;

    public TungiasisController(EventoTungiasisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/malaria")]
public class MalariaController : ControllerBase
{
    private readonly EventoMalariaUseCase _useCase;

    public MalariaController(EventoMalariaUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/tuberculosis")]
public class TuberculosisController : ControllerBase
{
    private readonly EventoTuberculosisUseCase _useCase;

    public TuberculosisController(EventoTuberculosisUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/tuberculosiscontacto")]
public class TuberculosisContactoController : ControllerBase
{
    private readonly EventoTuberculosisContactoUseCase _useCase;

    public TuberculosisContactoController(EventoTuberculosisContactoUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

[Authorize]
[ApiController]
[Route("api/leshmaniasiscutanea")]
public class LeshmaniasisCutaneaController : ControllerBase
{
    private readonly EventoLeshmaniasisCutaneaUseCase _useCase;

    public LeshmaniasisCutaneaController(EventoLeshmaniasisCutaneaUseCase useCase) => _useCase = useCase;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _useCase.GetAllAsync(ct));

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _useCase.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
