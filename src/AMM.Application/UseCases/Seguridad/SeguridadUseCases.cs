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
        GetAllAsync(e => new UsuarioDto(e.Id, e.Correo, e.NombreCompleto, e.EstadoUsuarioId, e.ModificadoEn ?? e.CreadoEn), ct);

    public Task<UsuarioDto?> GetUsuarioByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new UsuarioDto(e.Id, e.Correo, e.NombreCompleto, e.EstadoUsuarioId, e.ModificadoEn ?? e.CreadoEn), ct);

    public async Task<UsuarioDto?> GetByCorreoAsync(string correo, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(correo, ct);
        return usuario != null ? new UsuarioDto(usuario.Id, usuario.Correo, usuario.NombreCompleto, usuario.EstadoUsuarioId, usuario.ModificadoEn ?? usuario.CreadoEn) : null;
    }

    public async Task<UsuarioDto> CreateAsync(CrearUsuarioRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = new Usuario
        {
            Correo = request.Correo,
            NombreCompleto = request.NombreCompleto,
            EstadoUsuarioId = request.EstadoUsuarioId,
            AzureAdObjectId = request.AzureAdObjectId,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = usuarioActual,
            EstadoId = 1 // Estado activo por defecto
        };

        var created = await _usuarioRepository.AddAsync(entity, ct);
        await _usuarioRepository.SaveChangesAsync(ct);
        
        var result = await _usuarioRepository.GetByIdAsync(created.Id, ct);
        return new UsuarioDto(result!.Id, result.Correo, result.NombreCompleto, result.EstadoUsuarioId, result.ModificadoEn ?? result.CreadoEn);
    }

    public async Task UpdateAsync(UpdateUsuarioRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await _usuarioRepository.GetByIdAsync(request.Id, ct);
        if (entity == null) throw new KeyNotFoundException($"Usuario con ID {request.Id} no encontrado");

        entity.Correo = request.Correo;
        entity.NombreCompleto = request.NombreCompleto;
        entity.EstadoUsuarioId = request.EstadoUsuarioId;
        entity.AzureAdObjectId = request.AzureAdObjectId;
        entity.EntidadId = request.EntidadId;
        entity.ModificadoEn = DateTime.UtcNow;
        entity.ModificadoPor = usuarioActual;

        await _usuarioRepository.UpdateAsync(entity, ct);
        await _usuarioRepository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await _usuarioRepository.GetByIdAsync(id, ct);
        if (entity == null) throw new KeyNotFoundException($"Usuario con ID {id} no encontrado");

        // Soft delete: cambiar estado a inactivo
        entity.EstadoId = 2; 
        entity.ModificadoEn = DateTime.UtcNow;
        entity.ModificadoPor = usuarioActual;

        await _usuarioRepository.UpdateAsync(entity, ct);
        await _usuarioRepository.SaveChangesAsync(ct);
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
