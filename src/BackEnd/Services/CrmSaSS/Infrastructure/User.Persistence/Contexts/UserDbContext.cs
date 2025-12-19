using Microsoft.EntityFrameworkCore;
using Shared.Core.Data;

namespace User.Persistence.Contexts
{
    /// <summary>
    /// Represents the Entity Framework Core database context for user‑related entities.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="BaseDbContext"/> to enforce shared conventions such as
    /// GUID primary keys and snake_case naming. This context defines DbSets for
    /// <c>User</c> and <c>UserHistory</c> entities and applies configuration classes
    /// from the current assembly.
    /// </remarks>
    public class UserDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> representing application users.
        /// </summary>
        public DbSet<Domain.User> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> representing historical audit records
        /// of user lifecycle events (e.g., creation, activation, deactivation).
        /// </summary>
        public DbSet<Domain.UserHistory> UserHistories { get; set; }

        /// <summary>
        /// Configures the EF Core model for the <see cref="UserDbContext"/>.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> used to configure entity mappings.
        /// </param>
        /// <remarks>
        /// <para>
        /// Calls the base implementation to apply shared contributor‑safe conventions
        /// (GUID PKs, snake_case naming).
        /// </para>
        /// <para>
        /// Then applies all <see cref="IEntityTypeConfiguration{TEntity}"/> classes
        /// defined in the current assembly, ensuring context‑specific mappings
        /// (such as enum conversions or property constraints) are applied consistently.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply shared conventions from BaseDbContext
            base.OnModelCreating(modelBuilder);

            // Apply context-specific configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }
    }
}
