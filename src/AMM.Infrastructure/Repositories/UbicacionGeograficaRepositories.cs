using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

public class DepartamentoRepository : CatalogRepository<Departamento>, IDepartamentoRepository
{
    public DepartamentoRepository(AmmDbContext context) : base(context) { }
}

public class MunicipioRepository : CatalogRepository<Municipio>, IMunicipioRepository
{
    public MunicipioRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Municipio>> GetByDepartamentoIdAsync(short departamentoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.DepartamentoId == departamentoId)
            .Include(m => m.Departamento)
            .ToListAsync(cancellationToken);
    }
}

public class TerritorioRepository : CatalogRepository<Territorio>, ITerritorioRepository
{
    public TerritorioRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Territorio>> GetByMunicipioIdAsync(int municipioId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.MunicipioId == municipioId)
            .Include(t => t.Municipio)
            .ToListAsync(cancellationToken);
    }
}

public class MicroterritorioRepository : CatalogRepository<Microterritorio>, IMicroterritorioRepository
{
    public MicroterritorioRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Microterritorio>> GetByTerritorioIdAsync(int territorioId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(m => m.TerritorioId == territorioId)
            .Include(m => m.Territorio)
            .ToListAsync(cancellationToken);
    }
}

public class AreaRepository : CatalogRepository<Area>, IAreaRepository
{
    public AreaRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Area>> GetByMicroterritorioIdAsync(int microterritorioId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.MicroterritorioId == microterritorioId)
            .Include(a => a.Microterritorio)
            .ToListAsync(cancellationToken);
    }
}

public class UbicacionRepository : CatalogRepository<Ubicacion>, IUbicacionRepository
{
    public UbicacionRepository(AmmDbContext context) : base(context) { }
}
