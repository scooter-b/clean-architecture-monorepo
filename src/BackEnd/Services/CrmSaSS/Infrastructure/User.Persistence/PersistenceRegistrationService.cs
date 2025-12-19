using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Contracts.Persistence;
using Shared.Core.Data.Extensions;
using Shared.Repositories;
using User.Application.Contracts.Persistence;
using User.Domain.Enums;
using User.Persistence.Contexts;
using User.Persistence.Repositories;

namespace User.Persistence
{
    /// <summary>
    /// Provides extension methods for registering persistence‑layer services
    /// into the dependency injection container.
    /// </summary>
    /// <remarks>
    /// This class centralizes the registration of database contexts and repositories,
    /// ensuring contributor‑safe conventions and clear ownership of persistence logic.
    /// </remarks>
    public static class PersistenceRegistrationService
    {
        /// <summary>
        /// Registers persistence services, including the <see cref="UserDbContext"/> and repositories,
        /// with the dependency injection container.
        /// </summary>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> to which persistence services will be added.
        /// </param>
        /// <param name="configuration">
        /// The application configuration used to retrieve the database connection string.
        /// </param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance, enabling fluent chaining of service registrations.
        /// </returns>
        /// <remarks>
        /// This method performs the following registrations:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <b>UserDbContext:</b> Registered via <c>AddBaseDbContext</c>, enforcing shared conventions
        /// (GUID PKs, snake_case naming, UTC timestamps) and mapping the <c>UserEventType</c> enum
        /// to the PostgreSQL enum type <c>user_event_type</c>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>GenericRepository:</b> Registered as a scoped service for all entity types via
        /// <see cref="IGenericRepository{T}"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>UserRepository:</b> Registered as a scoped service for user‑specific persistence logic.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>UserHistoryRepository:</b> Registered as a scoped service for user history persistence logic.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            // Use the generic extension and map context-specific enum.
            services.AddBaseDbContext<UserDbContext>(
                connectionString!,
                npgsql =>
                {
                    // Map the C# enum UserEventType to PostgreSQL enum type "user_event_type"
                    npgsql.MapEnum<UserEventType>(typeof(UserEventType).Name.ToSnakeCase());
                });

            // Register generic repository for all entities
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register user-specific repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

            return services;
        }
    }
}
