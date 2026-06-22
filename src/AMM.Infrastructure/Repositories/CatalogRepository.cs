using System.Linq.Expressions;
using System.Reflection;
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
        // EF.Property<object> cannot be translated to SQL ORDER BY.
        // Use reflection to build a typed OrderBy expression from the actual PK type.
        var keyProp = _context.Model.FindEntityType(typeof(T))
            ?.FindPrimaryKey()?.Properties.FirstOrDefault();

        IQueryable<T> query = _dbSet.AsNoTracking();

        if (keyProp?.PropertyInfo is PropertyInfo pi)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var body  = Expression.Property(param, pi);
            var lambda = Expression.Lambda(body, param);
            var orderByMethod = typeof(Queryable)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == nameof(Queryable.OrderBy) && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), pi.PropertyType);
            query = (IQueryable<T>)orderByMethod.Invoke(null, [query, lambda])!;
        }

        return await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
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
