using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Repositories.Interfaces;
using Shared.Repositories;
using User.Application.Interfaces;
using User.Persistence.Repositories;

namespace User.Persistence
{
    public static class PersistenceRegistrationService
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext and other persistence-related services here
            services.AddDbContext<Contexts.UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

            return services;
        }
    }
}
