using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;
using System.Text.RegularExpressions;

namespace Shared.Core.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring EF Core <see cref="ModelBuilder"/>
    /// with standardized conventions for entity keys, table/column naming, and constraints.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies base entity configuration conventions to all entities in the model.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> instance used to configure entity mappings.
        /// </param>
        /// <remarks>
        /// <para>
        /// This method enforces contributor‑safe conventions across all entities:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <b>Structural Logic:</b> Ensures that all non‑abstract entities inheriting from
        /// <c>BaseEntity</c> use a GUID primary key named <c>Id</c>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>Table Naming Logic:</b> Converts table names to <c>snake_case</c> for PostgreSQL consistency.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>Properties Logic:</b> Converts column names to <c>snake_case</c> and automatically maps C# enums to PostgreSQL enum types (e.g., <c>UserStatus</c> → <c>user_status</c>).
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>Constraints and Indexes:</b> Converts key names, foreign key constraints, and index names to <c>snake_case</c>.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public static void ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // 1. Structural Logic: Enforce GUID PK for BaseEntity types
                if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType) && !entity.ClrType.IsAbstract)
                {
                    modelBuilder.Entity(entity.ClrType).HasKey(nameof(BaseEntity.Id));
                }

                // 2. Table Naming Logic
                entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

                // 3. Properties (Columns) Logic - Consolidate into one loop
                foreach (var property in entity.GetProperties())
                {
                    // Apply Snake Case to column name
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());

                    // Automatically map ENUM column types
                    var type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                    if (type.IsEnum)
                    {
                        // This ensures C# UserStatus maps to PG "user_status"
                        property.SetColumnType(type.Name.ToSnakeCase());
                    }
                }

                // 4. Constraints & Indexes
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()?.ToSnakeCase());
                }

                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName()?.ToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
                }
            }
        }

        /// <summary>
        /// Converts a string from PascalCase or camelCase into snake_case.
        /// </summary>
        /// <param name="input">
        /// The input string to be converted.
        /// </param>
        /// <returns>
        /// The converted string in snake_case format, or the original string if null or empty.
        /// </returns>
        /// <remarks>
        /// Example: <c>"UserStatus"</c> → <c>"user_status"</c>.
        /// </remarks>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
        }
    }
}
