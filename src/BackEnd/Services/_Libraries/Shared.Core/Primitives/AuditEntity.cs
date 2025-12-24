using Shared.Core.Abstractions;
using Shared.Core.Constants;
using Shared.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Shared.Core.Primitives
{
    /// <summary>
    /// An abstract base class for entities requiring comprehensive audit tracking.
    /// Integrates <see cref="AuditPrincipal"/> value objects to ensure high-integrity 
    /// identity tracking for creation, modification, and deletion events.
    /// </summary>
    public abstract class AuditEntity : BaseEntity, IAuditable
    {
        protected AuditEntity(AuditPrincipal createdBy, Guid? id = null, DateTime? createdAtUtc = null)
            : base(id)
        {
            CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
            CreatedBy = createdBy;

            // Standard: On creation, modification data matches creation data
            ModifiedAtUtc = CreatedAtUtc;
            ModifiedBy = CreatedBy;
        }

        /// <summary>
        /// The parameterless constructor is required for Entity Framework Core materialization.
        /// </summary>
#pragma warning disable CS8618
        protected AuditEntity() : base() { }
#pragma warning restore CS8618

        [Required]
        public DateTime CreatedAtUtc { get; private set; }

        [Required]
        [MaxLength(EntityConfigurationConstants.AuditPrincipal.MaxLength)]
        public AuditPrincipal CreatedBy { get; private set; } = null!;

        public DateTime? ModifiedAtUtc { get; private set; }

        [MaxLength(EntityConfigurationConstants.AuditPrincipal.MaxLength)]
        public AuditPrincipal? ModifiedBy { get; private set; }

        public DateTime? DeletedAtUtc { get; private set; }

        [MaxLength(EntityConfigurationConstants.AuditPrincipal.MaxLength)]
        public AuditPrincipal? DeletedBy { get; private set; }


        // --- Scoped Update Methods ---

        /// <summary>
        /// Updates the modification audit state.
        /// </summary>
        public void SetModified(AuditPrincipal modifiedBy)
            => UpdateModifiedAudit(modifiedBy);

        /// <summary>
        /// Updates the deletion audit state (Soft Delete).
        /// </summary>
        public void SetDeleted(AuditPrincipal deletedBy)
        {
            var now = DateTime.UtcNow;
            DeletedAtUtc = now;
            DeletedBy = deletedBy;

            // Also update modification state to reflect the state change
            UpdateModifiedAudit(deletedBy, now);
        }

        // --- Private Helper to maintain internal state ---

        private void UpdateModifiedAudit(AuditPrincipal principal, DateTime? atUtc = null)
        {
            ModifiedBy = principal;
            ModifiedAtUtc = atUtc ?? DateTime.UtcNow;
        }
    }
}
