using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities;

namespace User.Persistence.Configurations
{
    public class UserAccountLogConfiguration : IEntityTypeConfiguration<UserAccountLog>
    {
        public void Configure(EntityTypeBuilder<UserAccountLog> builder)
        {
        }
    }
}
