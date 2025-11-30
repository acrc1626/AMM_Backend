using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

public class UsuarioRepository : CatalogRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AmmDbContext context) : base(context) { }

    public async Task<Usuario?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Correo == correo, cancellationToken);
    }
}

public class RolRepository : CatalogRepository<Rol>, IRolRepository
{
    public RolRepository(AmmDbContext context) : base(context) { }
}

public class PermisoRepository : CatalogRepository<Permiso>, IPermisoRepository
{
    public PermisoRepository(AmmDbContext context) : base(context) { }
}

public class MenuRepository : CatalogRepository<Menu>, IMenuRepository
{
    public MenuRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Menu>> GetByRolIdAsync(int rolId, CancellationToken cancellationToken = default)
    {
        // Assuming there is a navigation property or join table for Rol-Menu
        // For now, basic implementation or TODO if complex join needed
        // Since we have many-to-many, we might need to access via context directly if navigation not fully exposed in generic way
        // But let's assume standard access for now or just return all for simplicity if navigation isn't simple
        
        // Better implementation accessing the join
        return await _context.Roles
            .Where(r => r.Id == rolId)
            .SelectMany(r => r.Menus)
            .ToListAsync(cancellationToken);
    }
}
