using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ED.Assistant.Data.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool tracking = false, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    IQueryable<TEntity> AsNoTracking();

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    
    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}