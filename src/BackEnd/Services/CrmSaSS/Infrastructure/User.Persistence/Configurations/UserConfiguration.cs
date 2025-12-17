using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Domain.User>
    {
        public void Configure(EntityTypeBuilder<Domain.User> builder)
        {
            builder.ToTable("users");

            builder.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(100);

            // Email property: always stored lowercase
            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired()
                .HasConversion(
                    v => v.ToLowerInvariant(),  // normalize before saving
                    v => v                      // no change when reading
                );

            // Unique index on LOWER(email) for case-insensitive uniqueness
            builder.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("ix_users_email_lower");

            // Computed column: "last_name, first_name"
            builder.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasComputedColumnSql(@"""first_name"" || ', ' || ""last_name""", stored: true);

            // One-to-many relationship
            builder.HasMany(e => e.Histories)
                .WithOne(h => h.User)
                .HasForeignKey(h => h.UserId)
                .HasConstraintName("fk_user_history_user");
        }
    }
}
