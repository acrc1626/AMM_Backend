using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

public interface IEventoRepository : ICatalogRepository<Evento>
{
    Task<IReadOnlyList<Evento>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default);
}

public interface IEventoEscabiosisRepository : ICatalogRepository<EventoEscabiosis> { }
public interface IEventoGeohelmintiasisRepository : ICatalogRepository<EventoGeohelmintiasis> { }
public interface IEventoPediculosisRepository : ICatalogRepository<EventoPediculosis> { }
public interface IEventoPianRepository : ICatalogRepository<EventoPian> { }
public interface IEventoTeniasisCisticercosisRepository : ICatalogRepository<EventoTeniasisCisticercosis> { }
public interface IEventoTracomaRepository : ICatalogRepository<EventoTracoma> { }
public interface IEventoTungiasisRepository : ICatalogRepository<EventoTungiasis> { }
public interface IEventoMalariaRepository : ICatalogRepository<EventoMalaria> { }
public interface IEventoTuberculosisRepository : ICatalogRepository<EventoTuberculosis> { }
public interface IEventoTuberculosisContactoRepository : ICatalogRepository<EventoTuberculosisContacto> { }
public interface IEventoLeshmaniasisCutaneaRepository : ICatalogRepository<EventoLeshmaniasisCutanea> { }
