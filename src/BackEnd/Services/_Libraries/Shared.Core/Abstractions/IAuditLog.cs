using Shared.Core.ValueObjects;

namespace Shared.Core.Abstractions
{
    /// <summary>
    /// Defines the core contract for system-wide audit trails.
    /// Adheres to 2025 Structured Event Taxonomy standards for observability and compliance.
    /// </summary>
    public interface IAuditLog
    {
        /// <summary>
        /// The unique, validated identity of the actor (Human or System) who initiated the event.
        /// Employs the "Type:Identifier" signature (e.g., "User:GUID") to ensure precise accountability.
        /// </summary>
        AuditPrincipal PerformedBy { get; }

        /// <summary>
        /// The precise moment the event occurred in Coordinated Universal Time (UTC).
        /// Standardized to UTC to ensure accurate chronological sequencing across distributed services.
        /// </summary>
        DateTime PerformedAtUtc { get; }

        /// <summary>
        /// The hierarchical, machine-readable name of the event.
        /// Uses dot-notation (e.g., "User.Email.Update") to facilitate granular filtering and log analytics.
        /// </summary>
        AuditAction Action { get; }

        /// <summary>
        /// A JSON-serialized snapshot of the entity's state prior to the modification.
        /// Essential for "Point-in-Time" recovery and detailed forensic "Before/After" comparisons.
        /// </summary>
        string? OriginalValues { get; }

        /// <summary>
        /// A JSON-serialized snapshot of the entity's state following the modification.
        /// Used to track the delta of changes and verify the final state of the record.
        /// </summary>
        string? NewValues { get; }
    }
}
