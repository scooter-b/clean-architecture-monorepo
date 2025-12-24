using Shared.Core.Abstractions;
using Shared.Core.Constants;
using Shared.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Shared.Core.Primitives
{
    /// <summary>
    /// A foundational base class for persistent audit log entries.
    /// Implements <see cref="IAuditLog"/> to enforce enterprise-grade event taxonomy and accountability.
    /// </summary>
    public abstract class AuditLogEntity : BaseEntity, IAuditLog
    {
        /// <summary>
        /// The validated identity of the human or system actor responsible for the event.
        /// Maps to a single database column via the AuditPrincipalConverter.
        /// </summary>
        [Required]
        [MaxLength(EntityConfigurationConstants.AuditPrincipal.MaxLength)]
        public AuditPrincipal PerformedBy { get; private set; }

        /// <summary>
        /// The timestamp of the event in Coordinated Universal Time (UTC).
        /// Initialized at the moment of instantiation to ensure chronological accuracy.
        /// </summary>
        [Required]
        public DateTime PerformedAtUtc { get; private set; }

        /// <summary>
        /// The hierarchical name of the business action (e.g., "User.Registration.Create").
        /// Enforces the dot-notation signature required for granular log analytics.
        /// </summary>
        [Required]
        public AuditAction Action { get; private set; }

        /// <summary>
        /// A snapshot of the resource state prior to the operation. 
        /// Typically stored as a JSON string for deep forensic analysis.
        /// </summary>
        public string? OriginalValues { get; private set; }

        /// <summary>
        /// A snapshot of the resource state after the operation has been applied.
        /// Allows for delta calculations and audit verification.
        /// </summary>
        public string? NewValues { get; private set; }

        /// <summary>
        /// Required by Entity Framework Core for materialization from the database.
        /// </summary>
#pragma warning disable CS8618
        protected AuditLogEntity() : base() { }
#pragma warning restore CS8618

        /// <summary>
        /// Initializes a new audit record with required metadata.
        /// </summary>
        /// <param name="action">The validated hierarchical action (Category.Operation).</param>
        /// <param name="performedBy">The validated actor identity (Type:Identifier).</param>
        /// <param name="originalValues">The optional pre-state JSON snapshot.</param>
        /// <param name="newValues">The optional post-state JSON snapshot.</param>
        protected AuditLogEntity(
            AuditAction action,
            AuditPrincipal performedBy,
            string? originalValues = null,
            string? newValues = null)
            : base()
        {
            // Assignment ensures that business logic validation has already occurred 
            // within the AuditAction and AuditPrincipal factory methods.
            Action = action;
            PerformedBy = performedBy;
            OriginalValues = originalValues;
            NewValues = newValues;

            // Automated timestamping ensures the 'PerformedAtUtc' cannot be 
            // tampered with or backdated by the application layer.
            PerformedAtUtc = DateTime.UtcNow;
        }
    }
}
