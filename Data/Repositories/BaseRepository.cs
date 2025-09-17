using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Data.Helpers;
using System.Diagnostics;
using Data.Interfaces;
using Data.Contexts;

namespace Data.Repositories;

/// <summary>
/// Base repository for standard database operations.
/// Returns RepositoryResult objects to support business-layer error handling.
/// </summary>
public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction? _transaction = null;

    #region Transaction Management

    public virtual async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(ct);
    }

    public virtual async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public virtual async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    #endregion

    #region CRUD

    public virtual async Task<RepositoryResult<TEntity>> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        try
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<TEntity>.Failure($"Failed to add entity: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Tries to load by primary key. Works for any PK type (int, Guid, string, composite via anonymous object[]).
    /// </summary>
    public virtual async Task<RepositoryResult<TEntity>> GetByIdAsync(object id, CancellationToken ct = default)
    {
        try
        {
            var entity = await _dbSet.FindAsync(new object?[] { id }, ct);
            return entity != null
                ? RepositoryResult<TEntity>.Success(entity)
                : RepositoryResult<TEntity>.Failure("Entity not found", 404);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving {typeof(TEntity).Name} by id: {ex}");
            return RepositoryResult<TEntity>.Failure($"Failed to retrieve entity: {ex.Message}", 500);
        }
    }

    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var items = await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);

            return RepositoryResult<IEnumerable<TEntity>>.Success(items);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving all {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<IEnumerable<TEntity>>.Failure($"Failed to retrieve entities: {ex.Message}", 500);
        }
    }

    public virtual async Task<RepositoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> selector,
        bool orderByDescending = false,
        Expression<Func<TEntity, object>>? sortBy = null,
        Expression<Func<TEntity, bool>>? where = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (where != null)
                query = query.Where(where);

            if (includes?.Length > 0)
                foreach (var include in includes)
                    query = query.Include(include);

            if (sortBy != null)
                query = orderByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);

            var projected = await query.Select(selector).ToListAsync(ct);

            return new RepositoryResult<IEnumerable<TSelect>>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = projected
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving all {typeof(TEntity).Name} (projected): {ex}");
            return RepositoryResult<IEnumerable<TSelect>>.Failure($"Failed to retrieve entities: {ex.Message}", 500);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        CancellationToken ct = default)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (includeExpression != null)
                query = includeExpression(query);

            return await query.ToListAsync(ct);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving all {typeof(TEntity).Name} with details: {ex}");
            return Enumerable.Empty<TEntity>();
        }
    }

    /// <summary>
    /// Returns all entities that match a given predicate.
    /// </summary>
    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllByPredicateAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
    {
        try
        {
            var items = await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(ct);

            return RepositoryResult<IEnumerable<TEntity>>.Success(items);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving filtered {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<IEnumerable<TEntity>>.Failure($"Failed to retrieve entities: {ex.Message}", 500);
        }
    }

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(expression, ct);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving {typeof(TEntity).Name}: {ex}");
            return null;
        }
    }

    public virtual async Task<TEntity?> GetOneWithDetailsAsync(
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (includeExpression != null)
                query = includeExpression(query);

            return await query.FirstOrDefaultAsync(predicate, ct);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving {typeof(TEntity).Name} with details: {ex}");
            return null;
        }
    }

    public virtual async Task<RepositoryResult<TEntity>> UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(ct);
            return RepositoryResult<TEntity>.Success(entity);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Träffar bara om RowVersion är konfigurerad som concurrency-token via Fluent API.
            Debug.WriteLine($"Concurrency conflict updating {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<TEntity>.Failure("Concurrency conflict. Please reload and try again.", 409);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<TEntity>.Failure($"Failed to update entity: {ex.Message}", 500);
        }
    }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity, CancellationToken ct = default)
    {
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return RepositoryResult<bool>.Success(true);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Debug.WriteLine($"Concurrency conflict deleting {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<bool>.Failure("Concurrency conflict. It may already be deleted.", 409);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {typeof(TEntity).Name}: {ex}");
            return RepositoryResult<bool>.Failure($"Failed to delete entity: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// Delete by primary key (works with int for TrainingClassEntity).
    /// </summary>
    public virtual async Task<RepositoryResult<bool>> DeleteByIdAsync(object id, CancellationToken ct = default)
    {
        try
        {
            var entity = await _dbSet.FindAsync(new object?[] { id }, ct);
            if (entity is null)
                return RepositoryResult<bool>.Failure("Entity not found", 404);

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return RepositoryResult<bool>.Success(true);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Debug.WriteLine($"Concurrency conflict deleting {typeof(TEntity).Name} by id: {ex}");
            return RepositoryResult<bool>.Failure("Concurrency conflict. It may already be deleted.", 409);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {typeof(TEntity).Name} by id: {ex}");
            return RepositoryResult<bool>.Failure($"Failed to delete entity: {ex.Message}", 500);
        }
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate, ct);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking existence of {typeof(TEntity).Name}: {ex}");
            return false;
        }
    }

    #endregion
}
