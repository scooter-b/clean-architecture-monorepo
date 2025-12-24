using Microsoft.EntityFrameworkCore;
using Shared.Core.Constants;
using Shared.Core.Data.Converters;
using Shared.Core.Data.Extensions;
using Shared.Core.ValueObjects;

namespace Shared.Core.Data
{
    /// <summary>
    /// A reusable base database context that enforces organizational standards across all modules.
    /// Uses a .NET Primary Constructor to pass <see cref="DbContextOptions"/> to the underlying EF provider.
    /// </summary>
    /// <param name="options">The configuration options for this context (Connection strings, lazy loading, etc.).</param>
    public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
    {
        /// <summary>
        /// Configures global mapping rules and conventions for all entities in this context.
        /// These rules are applied to the entire model before specific entity configurations.
        /// </summary>
        /// <param name="configurationBuilder">The builder used to define global conventions.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // GLOBAL DATETIME STANDARDIZATION (UTC)
            // Ensures every DateTime property in the database is automatically converted 
            // to UTC when saving and marked as UTC when reading. This prevents time-zone 
            // bugs and ensures consistency across distributed systems.
            configurationBuilder.Properties<DateTime>()
                .HaveConversion<UtcDateTimeConverter>();

            // NULLABLE DATETIME HANDLING
            // Extends the UTC conversion logic to nullable DateTime fields, ensuring 
            // optional timestamps (like 'DeletedAt' or 'ModifiedAt') maintain UTC integrity.
            configurationBuilder.Properties<DateTime?>()
                .HaveConversion<NullableUtcDateTimeConverter>();

            // VALUE OBJECT MAPPING
            // Automatically treats every value object property as a string column 
            // rather than a separate table.
            configurationBuilder
                .Properties<AuditPrincipal>()
                // Uses the custom converter to pipe the record through the .Create() factory method.
                .HaveConversion<AuditPrincipalConverter>()
                // Enforces a consistent database schema by applying the same MaxLength 
                // to every column that stores an AuditPrincipal identity.
                .HaveMaxLength(EntityConfigurationConstants.AuditPrincipal.MaxLength);

            configurationBuilder
                .Properties<AuditAction>()
                // Uses the custom converter to pipe the dot-notation record (Category.Operation) 
                // into a primitive string column for database storage.
                .HaveConversion<AuditActionConverter>()
                // Enforces a consistent database schema by applying the same MaxLength 
                // to every column that stores an AuditAction identity.
                .HaveMaxLength(EntityConfigurationConstants.AuditAction.MaxLength);
        }

        /// <summary>
        /// Configures the shape, data types, and relationships of the database schema.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FRAMEWORK DEFAULTS
            // Calls the base implementation (IdentityDbContext or DbContext).
            // This must be called first so that standard EF and Identity tables 
            // are initialized before we apply our custom modifications.
            base.OnModelCreating(modelBuilder);

            // GLOBAL OVERRIDES (THE "LAST WORD" RULE)
            // We run this extension method last to ensure our 'BaseEntity' rules 
            // (such as Shadow Properties for CreatedAt/ModifiedAt or Global Query Filters 
            // for Soft Deletes) are applied to every entity in the model, overriding 
            // any configurations set in previous steps.
            modelBuilder.ApplyBaseEntityConfiguration();
        }
    }
}
