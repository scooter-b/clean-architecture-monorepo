using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Abstractions;
using Shared.Core.Persistence;

namespace Shared.Core.DependencyInjection
{
    public static class SharedRegistrationServices
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
        {
            // Register the Unit of Work
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));

            // Register generic read and write repositories
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadWriteRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(ReadWriteRepository<>));

            return services;
        }
    }
}
