using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

/// <summary>Repositorio de censos con consulta de detalle profundo.</summary>
public interface ICensoRepository : ICatalogRepository<Censo>
{
    /// <summary>
    /// Retorna el censo con todas sus personas, parentescos, estados y
    /// enfermedades cargadas mediante eager loading.
    /// </summary>
    Task<Censo?> GetDetalleAsync(long id, CancellationToken ct = default);

    /// <summary>
    /// Agrega personas a un censo existente sin reemplazar las ya registradas (DEF-AMM-004).
    /// </summary>
    Task AgregarPersonasAsync(IEnumerable<CensoPersona> personas, CancellationToken ct = default);
}

public interface ICensoNovedadRepository : ICatalogRepository<CensoNovedad>
{
    Task<IReadOnlyList<CensoNovedad>> GetByCensoIdAsync(long censoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CensoNovedad>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default);
}
