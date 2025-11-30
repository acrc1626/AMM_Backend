using AMM.Api.Controllers.Base;
using AMM.Application.DTOs.Seguridad;
using AMM.Application.UseCases.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : CatalogControllerBase<UsuarioDto, int>
{
    private readonly UsuarioUseCase _useCase;

    public UsuariosController(UsuarioUseCase useCase, ILogger<UsuariosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<UsuarioDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllUsuariosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<UsuarioDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetUsuarioByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("correo/{correo}")]
    public async Task<ActionResult<UsuarioDto>> GetByCorreo(string correo, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByCorreoAsync(correo, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class RolesController : CatalogControllerBase<RolDto, int>
{
    private readonly RolUseCase _useCase;

    public RolesController(RolUseCase useCase, ILogger<RolesController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<RolDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllRolesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<RolDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetRolByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class PermisosController : CatalogControllerBase<PermisoDto, int>
{
    private readonly PermisoUseCase _useCase;

    public PermisosController(PermisoUseCase useCase, ILogger<PermisosController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<PermisoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllPermisosAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<PermisoDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetPermisoByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class MenusController : CatalogControllerBase<MenuDto, int>
{
    private readonly MenuUseCase _useCase;

    public MenusController(MenuUseCase useCase, ILogger<MenusController> logger) : base(logger)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public override async Task<ActionResult<IReadOnlyList<MenuDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _useCase.GetAllMenusAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public override async Task<ActionResult<MenuDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetMenuByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("rol/{rolId}")]
    public async Task<ActionResult<IReadOnlyList<MenuDto>>> GetByRol(int rolId, CancellationToken cancellationToken)
    {
        var result = await _useCase.GetByRolAsync(rolId, cancellationToken);
        return Ok(result);
    }
}
