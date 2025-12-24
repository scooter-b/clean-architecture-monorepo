using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Core.Constants;
using Shared.Core.ValueObjects;
using User.Domain.Entities;

namespace User.Persistence.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            // Table and Key Configuration
            builder.HasKey(u => u.Id);

            // Value Object Conversions (for PersonName and EmailAddress)
            // This converts your domain Value Objects to raw strings for database storage.
            builder.Property(u => u.FirstName)
                .HasConversion(vo => vo.Value, s => PersonName.Create(s))
                .HasMaxLength(EntityConfigurationConstants.PersonName.MaxLength)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasConversion(vo => vo.Value, s => PersonName.Create(s))
                .HasMaxLength(EntityConfigurationConstants.PersonName.MaxLength)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasConversion(vo => vo.Value, s => EmailAddress.Create(s))
                .HasMaxLength(EntityConfigurationConstants.EmailAddress.MaxLength)
                .IsRequired();

            // Optional Emails
            builder.Property(u => u.PendingEmail)
                .HasConversion(vo => vo != null ? vo.Value : null, s => s != null ? EmailAddress.Create(s) : null)
                .HasMaxLength(EntityConfigurationConstants.EmailAddress.MaxLength);

            builder.Property(u => u.PreviousEmail)
                .HasConversion(vo => vo != null ? vo.Value : null, s => s != null ? EmailAddress.Create(s) : null)
                .HasMaxLength(EntityConfigurationConstants.EmailAddress.MaxLength);

            // Unique Index (Case-Insensitive)
            // Best practice in 2025: Explicitly define uniqueness.
            // Note: Collation depends on your provider (e.g., SQL_Latin1_General_CP1_CI_AS for SQL Server).
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Computed Property for FullName
            // This offloads the concatenation to the database for consistent querying.
            builder.Property(u => u.FullName)
                .HasComputedColumnSql("last_name || ', ' || first_name", stored: true);
        }
    }
}
