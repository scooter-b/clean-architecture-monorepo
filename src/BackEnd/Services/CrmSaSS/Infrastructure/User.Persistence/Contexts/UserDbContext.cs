using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities.Extensions;

namespace User.Persistence.Contexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<Domain.User> Users { get; set; }
        public DbSet<Domain.UserHistory> UserHistories { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply shared conventions
            modelBuilder.ApplySharedEntitiesConventions();

            modelBuilder.Entity<Domain.User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .IsRequired()
                    .HasMaxLength(100);

                // Email property: always stored lowercase
                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToLowerInvariant(),  // normalize before saving
                        v => v                      // no change when reading
                    );

                // Unique index on LOWER(email) for case-insensitive uniqueness
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("ix_users_email_lower");

                // Computed column: "last_name, first_name"
                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasComputedColumnSql(@"""first_name"" || ', ' || ""last_name""", stored: true);

                // One-to-many relationship
                entity.HasMany(e => e.Histories)
                    .WithOne(h => h.User)
                    .HasForeignKey(h => h.UserId)
                    .HasConstraintName("fk_user_history_user");
            });

            modelBuilder.Entity<Domain.UserHistory>(entity =>
            {
                entity.ToTable("user_histories");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .IsRequired();
            });
        }
    }
}
