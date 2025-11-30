using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

public interface IEventoRepository : ICatalogRepository<Evento> 
{
    Task<IReadOnlyList<Evento>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default);
}

public interface IEscabiosisRepository : ICatalogRepository<Escabiosis> { }
public interface IGeohelmintiasisRepository : ICatalogRepository<Geohelmintiasis> { }
public interface IPediculosisRepository : ICatalogRepository<Pediculosis> { }
public interface IMalariaRepository : ICatalogRepository<Malaria> { }
public interface ITuberculosisRepository : ICatalogRepository<Tuberculosis> { }
public interface ITuberculosisContactoRepository : ICatalogRepository<TuberculosisContacto> { }
public interface ILeshmaniasisCutaneaRepository : ICatalogRepository<LeshmaniasisCutanea> { }
