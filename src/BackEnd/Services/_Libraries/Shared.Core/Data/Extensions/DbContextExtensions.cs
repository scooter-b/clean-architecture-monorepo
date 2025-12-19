using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Shared.Core.Data.Extensions
{
    /// <summary>
    /// Provides extension methods for registering and configuring EF Core DbContexts
    /// with PostgreSQL (Npgsql) in a consistent, reusable way.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Adds a base <see cref="DbContext"/> of type <typeparamref name="TContext"/> to the service collection,
        /// configured to use PostgreSQL with shared enum mappings and optional context-specific options.
        /// </summary>
        /// <typeparam name="TContext">
        /// The concrete <see cref="DbContext"/> type to register with the dependency injection container.
        /// </typeparam>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which the DbContext will be added.
        /// </param>
        /// <param name="connectionString">
        /// The PostgreSQL connection string used to connect to the database.
        /// </param>
        /// <param name="npgsqlOptionsAction">
        /// An optional delegate that allows callers to configure additional Npgsql options,
        /// such as mapping context-specific enums or tuning provider settings.
        /// </param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance, enabling fluent chaining of service registrations.
        /// </returns>
        /// <remarks>
        /// This method ensures:
        /// <list type="number">
        /// <item>
        /// <description>Shared global enums are always mapped via <c>MapSharedEnums()</c>.</description>
        /// </item>
        /// <item>
        /// <description>Caller-provided enum mappings or options are applied if supplied.</description>
        /// </item>
        /// <item>
        /// <description>Migrations are tied to the assembly containing <typeparamref name="TContext"/>.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddBaseDbContext<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    // Always map shared global enums
                    npgsqlOptions.MapSharedEnums();

                    // Execute context-specific enum mappings passed from the caller
                    npgsqlOptionsAction?.Invoke(npgsqlOptions);

                    // Enable migrations in the specific persistence assembly
                    npgsqlOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName);
                }));

            return services;
        }
    }

}
