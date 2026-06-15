using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation for catalog entities.
/// Read operations use AsNoTracking for performance.
/// </summary>
public class CatalogRepository<T> : ICatalogRepository<T> where T : class
{
    protected readonly AmmDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public CatalogRepository(AmmDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetPagedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking()
            .OrderBy(e => EF.Property<object>(e, "Id"))
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
