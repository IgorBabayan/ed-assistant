using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ED.Assistant.Data.Repository;

internal class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _set;

    public Repository(AppDbContext context) => _set = context.Set<TEntity>();

    public ValueTask<TEntity?> FindAsync(object[] keyValues,
        CancellationToken cancellationToken = default) => _set.FindAsync(keyValues, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false, CancellationToken cancellationToken = default)
    {
        var query = tracking ? _set : _set.AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) => _set.AnyAsync(predicate, cancellationToken);

    public void Add(TEntity entity) => _set.Add(entity);
    public void Update(TEntity entity) => _set.Update(entity);
    public void Remove(TEntity entity) => _set.Remove(entity);

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => _set.AddAsync(entity, cancellationToken);

    public IQueryable<TEntity> AsNoTracking() => _set.AsNoTracking();
}