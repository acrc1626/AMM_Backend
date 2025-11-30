using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

public interface IDepartamentoRepository : ICatalogRepository<Departamento> { }
public interface IMunicipioRepository : ICatalogRepository<Municipio> 
{
    Task<IReadOnlyList<Municipio>> GetByDepartamentoIdAsync(short departamentoId, CancellationToken cancellationToken = default);
}
public interface ITerritorioRepository : ICatalogRepository<Territorio> 
{
    Task<IReadOnlyList<Territorio>> GetByMunicipioIdAsync(int municipioId, CancellationToken cancellationToken = default);
}
public interface IMicroterritorioRepository : ICatalogRepository<Microterritorio> 
{
    Task<IReadOnlyList<Microterritorio>> GetByTerritorioIdAsync(int territorioId, CancellationToken cancellationToken = default);
}
public interface IAreaRepository : ICatalogRepository<Area> 
{
    Task<IReadOnlyList<Area>> GetByMicroterritorioIdAsync(int microterritorioId, CancellationToken cancellationToken = default);
}
public interface IUbicacionRepository : ICatalogRepository<Ubicacion> { }
