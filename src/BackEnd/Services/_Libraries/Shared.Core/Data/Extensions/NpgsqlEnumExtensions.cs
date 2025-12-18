using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Shared.Core.Entities.Enums;

namespace Shared.Core.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring PostgreSQL enum mappings
    /// in Entity Framework Core using Npgsql.
    /// </summary>
    public static class NpgsqlEnumExtensions
    {
        /// <summary>
        /// Maps shared C# enums to PostgreSQL enum types for use with EF Core.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="NpgsqlDbContextOptionsBuilder"/> used to configure
        /// Npgsql-specific options for the DbContext.
        /// </param>
        /// <returns>
        /// The same <see cref="NpgsqlDbContextOptionsBuilder"/> instance, allowing
        /// for fluent chaining of configuration calls.
        /// </returns>
        /// <remarks>
        /// This method registers the <c>ActorType</c> enum with the PostgreSQL
        /// enum type <c>actor_type</c>. The mapping is generated dynamically by
        /// converting the CLR enum name (<c>ActorType</c>) into snake_case
        /// (<c>actor_type</c>), ensuring consistency with PostgreSQL naming
        /// conventions.
        /// </remarks>
        public static NpgsqlDbContextOptionsBuilder MapSharedEnums(this NpgsqlDbContextOptionsBuilder builder)
        {
            // Map the C# enum ActorType to the PostgreSQL enum type "actor_type".
            builder.MapEnum<ActorType>(typeof(ActorType).Name.ToSnakeCase());

            return builder;
        }
    }


}
