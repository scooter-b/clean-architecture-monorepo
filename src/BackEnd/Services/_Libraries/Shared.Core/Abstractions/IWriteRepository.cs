namespace Shared.Core.Abstractions
{
    /// <summary>
    /// Defines command/write operations for a specific entity type.
    /// Implementation assumes an external Unit of Work handles the transaction commit.
    /// </summary>
    public interface IWriteRepository<T> where T : class
    {
        /// <summary>
        /// Starts tracking the entity for insertion into the database.
        /// </summary>
        Task AddAsync(T entity, CancellationToken ct = default);

        /// <summary>
        /// Marks an existing entity as modified in the change tracker.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Marks an entity for deletion.
        /// </summary>
        void Remove(T entity);
    }
}
