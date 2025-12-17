using Microsoft.EntityFrameworkCore;
using Shared.Entities.Configurations;

namespace Shared.Entities.Extensions
{
    public static class SharedEntitiesExtensions
    {
        public static void ApplySharedEntitiesConventions(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BaseEntityConfiguration());
        }

    }
}
