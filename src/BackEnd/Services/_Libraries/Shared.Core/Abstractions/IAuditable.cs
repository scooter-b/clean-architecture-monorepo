using Shared.Core.ValueObjects;

namespace Shared.Core.Abstractions
{
    /// <summary>
    /// Defines a standardized contract for entities requiring audit traceability.
    /// Uses <see cref="AuditPrincipal"/> to ensure identity integrity across creation, 
    /// modification, and deletion states.
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// The UTC timestamp when the entity was created.
        /// </summary>
        DateTime CreatedAtUtc { get; }

        /// <summary>
        /// The validated identity of the principal who created this entity.
        /// </summary>
        AuditPrincipal CreatedBy { get; }

        /// <summary>
        /// The UTC timestamp when the entity was modified.
        /// </summary>
        DateTime? ModifiedAtUtc { get; }

        /// <summary>
        /// The validated identity of the principal who modified this entity.
        /// </summary>
        AuditPrincipal? ModifiedBy { get; }

        /// <summary>
        /// The UTC timestamp when the entity was deleted.
        /// </summary>
        DateTime? DeletedAtUtc { get; }

        /// <summary>
        /// The validated identity of the principal who deleted this entity.
        /// </summary>
        AuditPrincipal? DeletedBy { get; }
    }
}
