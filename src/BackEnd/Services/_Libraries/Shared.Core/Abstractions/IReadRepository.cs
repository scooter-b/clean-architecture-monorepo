using System.Linq.Expressions;

namespace Shared.Core.Abstractions
{
    /// <summary>
    /// Defines read-only data access operations for a specific entity type.
    /// Implementation should typically use 'AsNoTracking' for performance.
    /// </summary>
    /// <typeparam name="T">The type of the domain entity.</typeparam>
    public interface IReadRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <remarks>
        /// Note: This method uses FindAsync, which checks the local tracker and tracks the entity.
        /// If strict AsNoTracking is required, override this or use FirstOrDefaultAsync.
        /// </remarks>
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Retrieves all entities of the specified type. 
        /// </summary>
        /// <remarks>
        /// This method uses 'AsNoTracking' for high performance. 
        /// Warning: For large tables, this may impact memory; consider using a paged alternative for production scale.
        /// </remarks>
        /// <param name="ct">The cancellation token to abort the request.</param>
        /// <returns>A read-only list of all entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Searches for the first entity that matches the specified filter criteria.
        /// </summary>
        /// <param name="predicate">A lambda expression to filter the entities (e.g., u => u.Email == email).</param>
        /// <param name="ct">The cancellation token to abort the query.</param>
        /// <returns>
        /// The matching entity if found; otherwise, null. 
        /// The returned entity is NOT tracked by the change tracker.
        /// </returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

        /// <summary>
        /// Efficiently checks if any entities exist that match the specified filter.
        /// </summary>
        /// <remarks>
        /// This is preferred over 'FirstOrDefaultAsync' or 'CountAsync' when only 
        /// existence verification is required. It performs a high-performance SQL 'EXISTS' check.
        /// </remarks>
        /// <param name="predicate">The filter criteria.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>True if at least one match exists; otherwise, false.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}
