using AMM.Application.DTOs.Seguridad;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Seguridad;

public class UsuarioUseCase : CatalogoUseCase<Usuario, UsuarioDto, CrearUsuarioRequest, int>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioUseCase(IUsuarioRepository repository) : base(repository) 
    {
        _usuarioRepository = repository;
    }

    public Task<IReadOnlyList<UsuarioDto>> GetAllUsuariosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new UsuarioDto(e.Id, e.Correo, e.NombreCompleto, e.EstadoUsuarioId), ct);

    public Task<UsuarioDto?> GetUsuarioByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new UsuarioDto(e.Id, e.Correo, e.NombreCompleto, e.EstadoUsuarioId), ct);

    public async Task<UsuarioDto?> GetByCorreoAsync(string correo, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(correo, ct);
        return usuario != null ? new UsuarioDto(usuario.Id, usuario.Correo, usuario.NombreCompleto, usuario.EstadoUsuarioId) : null;
    }
}

public class RolUseCase : CatalogoUseCase<Rol, RolDto, CrearRolRequest, int>
{
    public RolUseCase(IRolRepository repository) : base(repository) { }

    public Task<IReadOnlyList<RolDto>> GetAllRolesAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new RolDto(e.Id, e.Nombre, e.Descripcion), ct);

    public Task<RolDto?> GetRolByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new RolDto(e.Id, e.Nombre, e.Descripcion), ct);
}

public class PermisoUseCase : CatalogoUseCase<Permiso, PermisoDto, CrearPermisoRequest, int>
{
    public PermisoUseCase(IPermisoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<PermisoDto>> GetAllPermisosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new PermisoDto(e.Id, e.Codigo, e.Descripcion), ct);

    public Task<PermisoDto?> GetPermisoByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new PermisoDto(e.Id, e.Codigo, e.Descripcion), ct);
}

public class MenuUseCase : CatalogoUseCase<Menu, MenuDto, CrearMenuRequest, int>
{
    private readonly IMenuRepository _menuRepository;

    public MenuUseCase(IMenuRepository repository) : base(repository) 
    {
        _menuRepository = repository;
    }

    public Task<IReadOnlyList<MenuDto>> GetAllMenusAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new MenuDto(e.Id, e.Nombre, e.Ruta), ct);

    public Task<MenuDto?> GetMenuByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new MenuDto(e.Id, e.Nombre, e.Ruta), ct);
    
    public async Task<IReadOnlyList<MenuDto>> GetByRolAsync(int rolId, CancellationToken ct = default)
    {
        var menus = await _menuRepository.GetByRolIdAsync(rolId, ct);
        return menus.Select(e => new MenuDto(e.Id, e.Nombre, e.Ruta)).ToList();
    }
}
