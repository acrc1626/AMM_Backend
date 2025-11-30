using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

public class CensoRepository : CatalogRepository<Censo>, ICensoRepository
{
    public CensoRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Censo>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.PacienteId == pacienteId)
            .Include(c => c.TipoEntorno)
            .Include(c => c.Paciente)
            .Include(c => c.Ubicacion)
            .Include(c => c.Departamento)
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IReadOnlyList<Censo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.TipoEntorno)
            .Include(c => c.Paciente)
            .Include(c => c.Ubicacion)
            .Include(c => c.Departamento)
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Censo?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        if (id is long longId)
        {
            return await _dbSet
                .Include(c => c.TipoEntorno)
                .Include(c => c.Paciente)
                .Include(c => c.Ubicacion)
                .Include(c => c.Departamento)
                .Include(c => c.Municipio)
                .Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Id == longId, cancellationToken);
        }
        return null;
    }
}

public class CensoNovedadRepository : CatalogRepository<CensoNovedad>, ICensoNovedadRepository
{
    public CensoNovedadRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<CensoNovedad>> GetByCensoIdAsync(long censoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(cn => cn.CensoId == censoId)
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CensoNovedad>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(cn => cn.PacienteId == pacienteId)
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IReadOnlyList<CensoNovedad>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .ToListAsync(cancellationToken);
    }

    public override async Task<CensoNovedad?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        if (id is long longId)
        {
            return await _dbSet
                .Include(cn => cn.TipoNovedad)
                .Include(cn => cn.PresenciaNovedad)
                .Include(cn => cn.Paciente)
                .Include(cn => cn.Estado)
                .FirstOrDefaultAsync(cn => cn.Id == longId, cancellationToken);
        }
        return null;
    }
}
