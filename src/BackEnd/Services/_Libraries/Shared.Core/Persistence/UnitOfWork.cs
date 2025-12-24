using Microsoft.EntityFrameworkCore;
using Shared.Core.Abstractions;
using Shared.Core.Data;

namespace Shared.Core.Persistence
{
    /// <inheritdoc cref="IUnitOfWork" />
    /// <remarks>
    /// This implementation manages the save cycle for <see cref="BaseDbContext"/>.
    /// It ensures atomicity for all staged repository operations.
    /// </remarks>
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _context;
        private bool _disposed;

        public UnitOfWork(T context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        // Synchronous Cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Asynchronous Cleanup (Recommended for 2025/EF Core 9+)
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
