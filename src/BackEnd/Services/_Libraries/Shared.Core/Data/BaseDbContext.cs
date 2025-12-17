using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities.Enums;

namespace Shared.Core.Data
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<ActorType>();

            // Auto-apply IEntityTypeConfiguration<T> in derived assembly
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
