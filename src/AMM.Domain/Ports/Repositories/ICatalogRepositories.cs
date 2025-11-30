using AMM.Domain.Entities;

namespace AMM.Domain.Ports.Repositories;

/// <summary>
/// Generic repository port for catalog entities (read-only mostly)
/// </summary>
public interface ICatalogRepository<T> where T : class
{
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

// Specific interfaces for type safety
public interface IEstadoRepository : ICatalogRepository<Estado> { }
public interface IEstadoUsuarioRepository : ICatalogRepository<EstadoUsuario> { }
public interface ITipoDocumentoRepository : ICatalogRepository<TipoDocumento> { }
public interface ISexoRepository : ICatalogRepository<Sexo> { }
public interface IEtniaRepository : ICatalogRepository<Etnia> { }
public interface IPuebloIndigenaRepository : ICatalogRepository<PuebloIndigena> { }
public interface ITipoEntornoRepository : ICatalogRepository<TipoEntorno> { }
public interface ITipoNovedadRepository : ICatalogRepository<TipoNovedad> { }
public interface IPresenciaNovedadRepository : ICatalogRepository<PresenciaNovedad> { }
public interface ITipoSupervisionRepository : ICatalogRepository<TipoSupervision> { }
public interface IMotivoNoTratamientoRepository : ICatalogRepository<MotivoNoTratamiento> { }
public interface IParentescoRepository : ICatalogRepository<Parentesco> { }
public interface IEventoTipoRepository : ICatalogRepository<EventoTipo> { }
public interface IFormaFarmaceuticaRepository : ICatalogRepository<FormaFarmaceutica> { }
