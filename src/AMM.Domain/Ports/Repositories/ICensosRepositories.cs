using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

public interface ICensoRepository : ICatalogRepository<Censo> 
{
    Task<IReadOnlyList<Censo>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default);
}

public interface ICensoNovedadRepository : ICatalogRepository<CensoNovedad> 
{
    Task<IReadOnlyList<CensoNovedad>> GetByCensoIdAsync(long censoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CensoNovedad>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default);
}
