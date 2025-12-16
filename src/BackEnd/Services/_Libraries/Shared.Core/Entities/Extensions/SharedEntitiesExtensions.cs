using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities.Configurations;

namespace Shared.Core.Entities.Extensions
{
    public static class SharedEntitiesExtensions
    {
        public static void ApplySharedEntitiesConventions(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BaseEntityConfiguration());
        }

    }
}
