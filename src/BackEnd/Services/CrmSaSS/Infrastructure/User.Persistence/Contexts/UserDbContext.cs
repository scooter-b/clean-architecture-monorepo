using Microsoft.EntityFrameworkCore;
using Shared.Core.Data;
using User.Domain.Enums;

namespace User.Persistence.Contexts
{
    public class UserDbContext : BaseDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<UserEventType>();
        }
    }
}
