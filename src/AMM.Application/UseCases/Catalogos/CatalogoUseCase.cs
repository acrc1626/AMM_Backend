using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Catalogos;

/// <summary>
/// Generic use case for catalog entities CRUD operations
/// </summary>
public class CatalogoUseCase<TEntity, TDto, TCreateRequest, TId> 
    where TEntity : class
{
    protected readonly ICatalogRepository<TEntity> _repository;

    public CatalogoUseCase(ICatalogRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<IReadOnlyList<TDto>> GetAllAsync(
        Func<TEntity, TDto> mapper, 
        CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(mapper).ToList();
    }

    public virtual async Task<TDto?> GetByIdAsync(
        TId id, 
        Func<TEntity, TDto> mapper, 
        CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity != null ? mapper(entity) : default;
    }

    public virtual async Task<TDto> CreateAsync(
        TCreateRequest request,
        Func<TCreateRequest, TEntity> factory,
        Func<TEntity, TDto> mapper,
        CancellationToken cancellationToken = default)
    {
        var entity = factory(request);
        var created = await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return mapper(created);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
