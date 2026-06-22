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

public class EventoEscabiosisRepository : CatalogRepository<EventoEscabiosis>, IEventoEscabiosisRepository
{
    public EventoEscabiosisRepository(AmmDbContext context) : base(context) { }
}

public class EventoGeohelmintiasisRepository : CatalogRepository<EventoGeohelmintiasis>, IEventoGeohelmintiasisRepository
{
    public EventoGeohelmintiasisRepository(AmmDbContext context) : base(context) { }
}

public class EventoPediculosisRepository : CatalogRepository<EventoPediculosis>, IEventoPediculosisRepository
{
    public EventoPediculosisRepository(AmmDbContext context) : base(context) { }
}

public class EventoPianRepository : CatalogRepository<EventoPian>, IEventoPianRepository
{
    public EventoPianRepository(AmmDbContext context) : base(context) { }
}

public class EventoTeniasisCisticercosisRepository : CatalogRepository<EventoTeniasisCisticercosis>, IEventoTeniasisCisticercosisRepository
{
    public EventoTeniasisCisticercosisRepository(AmmDbContext context) : base(context) { }
}

public class EventoTracomaRepository : CatalogRepository<EventoTracoma>, IEventoTracomaRepository
{
    public EventoTracomaRepository(AmmDbContext context) : base(context) { }
}

public class EventoTungiasisRepository : CatalogRepository<EventoTungiasis>, IEventoTungiasisRepository
{
    public EventoTungiasisRepository(AmmDbContext context) : base(context) { }
}

public class EventoMalariaRepository : CatalogRepository<EventoMalaria>, IEventoMalariaRepository
{
    public EventoMalariaRepository(AmmDbContext context) : base(context) { }
}

public class EventoTuberculosisRepository : CatalogRepository<EventoTuberculosis>, IEventoTuberculosisRepository
{
    public EventoTuberculosisRepository(AmmDbContext context) : base(context) { }
}

public class EventoTuberculosisContactoRepository : CatalogRepository<EventoTuberculosisContacto>, IEventoTuberculosisContactoRepository
{
    public EventoTuberculosisContactoRepository(AmmDbContext context) : base(context) { }
}

public class EventoLeshmaniasisCutaneaRepository : CatalogRepository<EventoLeshmaniasisCutanea>, IEventoLeshmaniasisCutaneaRepository
{
    public EventoLeshmaniasisCutaneaRepository(AmmDbContext context) : base(context) { }
}
