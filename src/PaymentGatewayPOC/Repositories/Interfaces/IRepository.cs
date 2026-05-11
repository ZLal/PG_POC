using System.Linq.Expressions;

namespace PaymentGatewayPOC.Repositories.Interfaces;

/// <summary>
/// Generic repository interface for common CRUD operations
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get entity by ID
    /// </summary>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Find entities based on predicate
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Add new entity
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Add multiple entities
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Update existing entity
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Delete entity by ID
    /// </summary>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Delete entity
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Delete multiple entities
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Check if entity exists
    /// </summary>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    /// Get count of all entities
    /// </summary>
    Task<int> CountAsync();

    /// <summary>
    /// Get count of entities matching predicate
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Check if any entity matches predicate
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
