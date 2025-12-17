using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Persistence.Configurations
{
    public class UserHistoryConfiguration : IEntityTypeConfiguration<Domain.UserHistory>
    {
        public void Configure(EntityTypeBuilder<Domain.UserHistory> builder)
        {
            builder.ToTable("user_histories");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(e => e.EventType)
                .HasColumnName("event_type")
                .HasColumnType("user_event_type")
                .IsRequired();
        }
    }
}
