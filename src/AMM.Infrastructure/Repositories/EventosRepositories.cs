using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

public class EventoRepository : CatalogRepository<Evento>, IEventoRepository
{
    public EventoRepository(AmmDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Evento>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.PacienteId == pacienteId)
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }

    public override async Task<IReadOnlyList<Evento>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class EscabiosisRepository : CatalogRepository<Escabiosis>, IEscabiosisRepository
{
    public EscabiosisRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Escabiosis>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class GeohelmintiasisRepository : CatalogRepository<Geohelmintiasis>, IGeohelmintiasisRepository
{
    public GeohelmintiasisRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Geohelmintiasis>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class PediculosisRepository : CatalogRepository<Pediculosis>, IPediculosisRepository
{
    public PediculosisRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Pediculosis>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class MalariaRepository : CatalogRepository<Malaria>, IMalariaRepository
{
    public MalariaRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Malaria>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class TuberculosisRepository : CatalogRepository<Tuberculosis>, ITuberculosisRepository
{
    public TuberculosisRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<Tuberculosis>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}

public class TuberculosisContactoRepository : CatalogRepository<TuberculosisContacto>, ITuberculosisContactoRepository
{
    public TuberculosisContactoRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<TuberculosisContacto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .Include(e => e.Index)
            .Include(e => e.Parentesco)
            .ToListAsync(cancellationToken);
    }
}

public class LeshmaniasisCutaneaRepository : CatalogRepository<LeshmaniasisCutanea>, ILeshmaniasisCutaneaRepository
{
    public LeshmaniasisCutaneaRepository(AmmDbContext context) : base(context) { }

    public override async Task<IReadOnlyList<LeshmaniasisCutanea>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.EventoTipo)
            .Include(e => e.Paciente)
            .Include(e => e.Estado)
            .ToListAsync(cancellationToken);
    }
}
