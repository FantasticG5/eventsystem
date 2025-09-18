using System.Linq.Expressions;
using Data.Helpers;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    // Transaction management
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);

    // CRUD
    Task<RepositoryResult<TEntity>> AddAsync(TEntity entity, CancellationToken ct = default);
    Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity, CancellationToken ct = default);
    Task<RepositoryResult<bool>> DeleteByIdAsync(object id, CancellationToken ct = default);

    // Reads
    Task<RepositoryResult<TEntity>> GetByIdAsync(object id, CancellationToken ct = default);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(CancellationToken ct = default);
    Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        CancellationToken ct = default);

    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);

    Task<TEntity?> GetOneWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}
