using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;
using Shared.Core.Repositories.Interfaces;

namespace Shared.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
