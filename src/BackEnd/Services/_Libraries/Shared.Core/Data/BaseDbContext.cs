using Microsoft.EntityFrameworkCore;
using Shared.Core.Data.Converters;
using Shared.Core.Data.Extensions;

namespace Shared.Core.Data
{
    /// <summary>
    /// Provides a base <see cref="DbContext"/> implementation that enforces
    /// shared conventions across all derived contexts.
    /// </summary>
    /// <remarks>
    /// This abstract base class ensures that common configuration logic
    /// (such as GUID primary keys and snake_case naming) is consistently
    /// applied to all entities. Derived DbContexts inherit these rules
    /// automatically.
    /// </remarks>
    public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
    {
        /// <summary>
        /// Configures global conventions for the EF Core model.
        /// </summary>
        /// <param name="configurationBuilder">
        /// The <see cref="ModelConfigurationBuilder"/> used to define conventions
        /// applied across all entities in the model.
        /// </param>
        /// <remarks>
        /// This override enforces contributor‑safe conventions for <see cref="DateTime"/> properties:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Applies a <c>UtcDateTimeConverter</c> to all non‑nullable <see cref="DateTime"/> properties,
        /// ensuring values are stored and retrieved in UTC.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Applies a <c>NullableUtcDateTimeConverter</c> to all nullable <see cref="DateTime"/> properties,
        /// ensuring consistent UTC handling even when values are optional.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// Standardizes the database column type for PostgreSQL as <c>timestamp with time zone</c>,
        /// aligning with PostgreSQL best practices for storing UTC timestamps.
        /// </description>
        /// </item>
        /// </list>
        /// By centralizing these rules, contributors don’t need to remember to apply converters
        /// or column types manually in each entity configuration.
        /// </remarks>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Apply the converters to ALL DateTime properties in the entire Model
            configurationBuilder.Properties<DateTime>()
                .HaveConversion<UtcDateTimeConverter>();

            configurationBuilder.Properties<DateTime?>()
                .HaveConversion<NullableUtcDateTimeConverter>();
        }


        /// <summary>
        /// Configures the EF Core model by applying base entity conventions.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> used to configure entity mappings.
        /// </param>
        /// <remarks>
        /// <para>
        /// The base implementation of <see cref="OnModelCreating"/> is invoked first
        /// to apply any framework or configuration logic.
        /// </para>
        /// <para>
        /// Finally, <c>ApplyBaseEntityConfiguration()</c> is executed to enforce
        /// contributor‑safe conventions such as:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>GUID primary keys for <c>BaseEntity</c> types.</description>
        /// </item>
        /// <item>
        /// <description>Snake_case naming for tables, columns, constraints, and indexes.</description>
        /// </item>
        /// </list>
        /// <para>
        /// This method is intentionally run last to override any conflicting
        /// configurations defined earlier.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base implementation to apply framework defaults
            base.OnModelCreating(modelBuilder);

            // RUN THIS LAST - It must override everything above
            modelBuilder.ApplyBaseEntityConfiguration();
        }
    }
}
