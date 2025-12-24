using Shared.Core.Constants;

namespace Shared.Core.ValueObjects
{
    /// <summary>
    /// A Value Object representing a validated identity (User or System) responsible for an action.
    /// Implemented as a record to provide built-in value-based equality.
    /// </summary>
    public record AuditPrincipal
    {
        /// <summary>
        /// Internal constructor allows the Persistence layer (ValueConverters) to materialize 
        /// the object from the database without re-triggering validation logic.
        /// </summary>
        /// <param name="value">The raw string value from the database.</param>
        internal AuditPrincipal(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Required by EF Core for proxy creation and certain materialization paths.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        protected AuditPrincipal() { }
#pragma warning restore CS8618

        /// <summary>
        /// The immutable identifier string stored in the database (e.g., "User:GUID" or "System:Name").
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Returns the raw identifier string, useful for logging and debugging.
        /// </summary>
        public override string ToString() => Value;


        /// <summary>
        /// Facilitates seamless integration by allowing the object to be used 
        /// where a string is expected (e.g., LINQ queries, Logging).
        /// </summary>
        public static implicit operator string(AuditPrincipal principal) => principal?.Value ?? string.Empty;

        /// <summary>
        /// Factory method for human users. Enforces the "User:{GUID}" signature.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the GUID is empty.</exception>
        public static AuditPrincipal FromUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Audit user identifier cannot be an empty GUID.", nameof(userId));

            return new AuditPrincipal($"User:{userId}");
        }

        /// <summary>
        /// Factory method for automated services. Enforces the "System:{Name}" signature.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name is empty or exceeds database limits.</exception>
        public static AuditPrincipal FromSystem(string serviceName)
        {
            var normalized = serviceName?.Trim();

            if (string.IsNullOrWhiteSpace(normalized))
                throw new ArgumentException("Audit system/service name cannot be empty.", nameof(serviceName));

            // Construct full signature to validate against schema MaxLength
            var finalValue = $"System:{normalized}";

            if (finalValue.Length > EntityConfigurationConstants.AuditPrincipal.MaxLength)
                throw new ArgumentException(
                    $"System identity exceeds maximum length of {EntityConfigurationConstants.AuditPrincipal.MaxLength}.",
                    nameof(serviceName));

            return new AuditPrincipal(finalValue);
        }


    }
}
