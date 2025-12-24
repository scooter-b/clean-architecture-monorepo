using Microsoft.EntityFrameworkCore;

namespace Shared.Core.Abstractions
{
    /// <summary>
    /// Coordinates atomic transactions across all shared repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Persists all changes tracked by the shared context to the database as a single transaction.
        /// </summary>
        /// <remarks>
        /// This is the final step in a repository workflow. If any operation fails, 
        /// no changes from the current unit of work will be committed.
        /// </remarks>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
