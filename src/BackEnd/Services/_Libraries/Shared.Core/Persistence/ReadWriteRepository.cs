using Microsoft.EntityFrameworkCore;
using Shared.Core.Abstractions;
using System.Linq.Expressions;

namespace Shared.Core.Persistence
{
    /// <inheritdoc />
    /// <remarks>
    /// This implementation relies on <see cref="DbContext"/> tracking. 
    /// Transactions must be committed via an external Unit of Work.
    /// </remarks>
    public class ReadWriteRepository<T> : IReadWriteRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public ReadWriteRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }


        // Read Methods

        /// <inheritdoc />
        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, ct);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public virtual async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, ct);
        }

        /// <inheritdoc />
        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(predicate, ct);
        }


        // Write Methods

        /// <inheritdoc />
        /// <remarks>
        /// This method only adds the entity to the tracker; no database call is made yet.
        /// </remarks>
        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Updates the entity state to 'Modified' in the local context.
        /// </remarks>
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <inheritdoc />
        /// <remarks>
        /// Marks the entity for deletion; it will be removed upon the next commit.
        /// </remarks>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
