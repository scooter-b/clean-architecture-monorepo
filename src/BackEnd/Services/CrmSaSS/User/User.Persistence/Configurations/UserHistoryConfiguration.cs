using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities;

namespace User.Persistence.Configurations
{
    /// <summary>
    /// Configures the EF Core entity mapping for <see cref="Domain.Entities.UserHistory"/>.
    /// </summary>
    /// <remarks>
    /// This configuration enforces contributor‑safe rules for the <c>UserHistory</c> entity:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>UserId:</b> Required property that links the history record to a specific user.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>EventType:</b> Required property that records the type of lifecycle event
    /// (e.g., Created, Activated, Deactivated).
    /// </description>
    /// </item>
    /// </list>
    /// By enforcing these constraints, the audit trail remains complete and reliable.
    /// </remarks>
    public class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistory>
    {
        /// <summary>
        /// Configures the <see cref="Domain.Entities.UserHistory"/> entity using the provided builder.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity.
        /// </param>
        /// <remarks>
        /// <para>
        /// Ensures that <c>UserId</c> and <c>EventType</c> are required fields,
        /// preventing incomplete audit records from being persisted.
        /// </para>
        /// </remarks>
        public void Configure(EntityTypeBuilder<UserHistory> builder)
        {
            // UserId must always be present to link the history record to a user
            builder.Property(e => e.UserId)
                .IsRequired();

            // EventType must always be present to describe the lifecycle event
            builder.Property(e => e.EventType)
                .IsRequired();
        }
    }
}
