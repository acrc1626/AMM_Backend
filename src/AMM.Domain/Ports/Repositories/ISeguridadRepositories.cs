using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

public interface IUsuarioRepository : ICatalogRepository<Usuario> 
{
    Task<Usuario?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default);
}

public interface IRolRepository : ICatalogRepository<Rol> { }

public interface IPermisoRepository : ICatalogRepository<Permiso> { }

public interface IMenuRepository : ICatalogRepository<Menu> 
{
    Task<IReadOnlyList<Menu>> GetByRolIdAsync(int rolId, CancellationToken cancellationToken = default);
}
