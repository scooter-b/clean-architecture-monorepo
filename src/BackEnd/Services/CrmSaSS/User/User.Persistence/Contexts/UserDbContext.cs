using Microsoft.EntityFrameworkCore;
using Shared.Core.Data;
using User.Domain.Entities;

namespace User.Persistence.Contexts
{
    /// <summary>
    /// The primary Database Context for the User module.
    /// Inherits shared conventions (UTC, AuditPrincipal, snake_case) from <see cref="BaseDbContext"/>.
    /// </summary>
    /// <param name="options">Configuration options provided by the Dependency Injection container.</param>
    public class UserDbContext(DbContextOptions<UserDbContext> options) : BaseDbContext(options)
    {
        /// <summary>
        /// Core account data including identity, status, and security metadata.
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Audit trail for user-related actions and administrative changes.
        /// </summary>
        public DbSet<UserAccountLog> UserAccountLogs { get; set; }

        /// <summary>
        /// Legacy user records maintained for migration or synchronization purposes.
        /// Uses explicit namespacing to avoid collision with modern User entities.
        /// </summary>
        public DbSet<Domain.Entities.User> Users { get; set; }

        /// <summary>
        /// Temporal history records allowing for point-in-time state recovery.
        /// </summary>
        public DbSet<UserHistory> UserHistories { get; set; }

        /// <summary>
        /// Configures the EF Core model specific to the User module.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the database schema.</param>
        /// <remarks>
        /// <para>
        /// 1. Calls <c>base.OnModelCreating</c> to enforce global standards (UTC, Auditing, Global Filters).
        /// </para>
        /// <para>
        /// 2. Uses Assembly Scanning to automatically find and apply all 
        /// <see cref="IEntityTypeConfiguration{TEntity}"/> implementations in this project.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply global architectural rules defined in the BaseDbContext
            base.OnModelCreating(modelBuilder);

            // AUTOMATED CONFIGURATION DISCOVERY:
            // Automatically registers all Fluent API configurations (e.g., UserAccountConfiguration)
            // located in this assembly. This prevents this file from becoming bloated as new entities are added.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }
    }
}
