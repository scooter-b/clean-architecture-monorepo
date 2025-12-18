using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Persistence.Configurations
{
    /// <summary>
    /// Configures the EF Core entity mapping for <see cref="Domain.User"/>.
    /// </summary>
    /// <remarks>
    /// This configuration enforces contributor‑safe rules for the <c>User</c> entity:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>FirstName / LastName:</b> Required properties with a maximum length of 100 characters.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Email:</b> Required property with a maximum length of 255 characters.
    /// Always normalized to lowercase before saving, ensuring consistent storage.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Email Index:</b> Unique index applied to <c>Email</c> for case‑insensitive uniqueness.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>FullName:</b> Computed column combining <c>last_name</c> and <c>first_name</c>
    /// in the format "last_name, first_name".
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Histories:</b> One‑to‑many relationship with <c>UserHistory</c>, linked by <c>UserId</c>.
    /// </description>
    /// </item>
    /// </list>
    /// These rules ensure data integrity, enforce normalization, and provide
    /// contributor‑safe conventions for onboarding.
    /// </remarks>
    public class UserConfiguration : IEntityTypeConfiguration<Domain.User>
    {
        /// <summary>
        /// Configures the <see cref="Domain.User"/> entity using the provided builder.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity.
        /// </param>
        /// <remarks>
        /// <para>
        /// Enforces required constraints on <c>FirstName</c>, <c>LastName</c>, and <c>Email</c>.
        /// </para>
        /// <para>
        /// Normalizes <c>Email</c> values to lowercase before saving, ensuring case‑insensitive uniqueness.
        /// </para>
        /// <para>
        /// Defines a computed column <c>FullName</c> and establishes a one‑to‑many relationship
        /// with <c>UserHistory</c>.
        /// </para>
        /// </remarks>
        public void Configure(EntityTypeBuilder<Domain.User> builder)
        {
            // FirstName: required, max length 100
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // LastName: required, max length 100
            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Email property: always stored lowercase
            builder.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired()
                .HasConversion(
                    v => v.ToLowerInvariant(),  // normalize before saving
                    v => v                      // no change when reading
                );

            // Unique index on LOWER(email) for case-insensitive uniqueness
            builder.HasIndex(e => e.Email)
                .IsUnique();

            // Computed column: "last_name, first_name"
            builder.Property(e => e.FullName)
                .HasComputedColumnSql("last_name || ', ' || first_name", stored: true);

            // One-to-many relationship: User → UserHistories
            builder.HasMany(e => e.Histories)
                .WithOne(h => h.User)
                .HasForeignKey(h => h.UserId);
        }
    }
}
