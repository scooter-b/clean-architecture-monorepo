using Shared.Core.Constants;
using System.Text.RegularExpressions;

namespace Shared.Core.ValueObjects
{
    /// <summary>
    /// Represents a validated, machine-readable Value Object for system event names.
    /// Employing a "Hierarchical Action Logging" pattern (Noun-Verb Namespace).
    /// </summary>
    /// <remarks>
    /// <para><b>Pattern: Category.[SubCategory].Operation</b></para>
    /// <para>
    /// This record enforces a structured taxonomy (e.g., "User.Registration.Create") 
    /// rather than free-text descriptions. This is the industry standard for 2025 
    /// observability, mirroring patterns used by OpenTelemetry and GitHub Audit Logs.
    /// </para>
    ///
    /// <para><b>Key Architectural Benefits:</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <b>Hierarchical Filtering:</b> Enables high-performance "Drill-Down" queries 
    /// in SQL or Log Analytics (e.g., <c>WHERE Action LIKE 'User.%'</c>).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Semantic Integrity:</b> Prevents fragmented data results caused by 
    /// "magic strings" or inconsistent naming (e.g., "User.Add" vs "User.Create").
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <b>Security Forensics:</b> Allows for granular alerting on specific 
    /// attributes (e.g., alerting only on "User.Email.Update" instead of all user changes).
    /// </description>
    /// </item>
    /// </list>
    ///
    /// <para><b>Signature Rules:</b></para>
    /// <list type="number">
    /// <item><description>Must be Alphanumeric (A-Z, 0-9).</description></item>
    /// <item><description>Segments must be separated by a single period (.).</description></item>
    /// <item><description>Must contain a minimum of 2 and a maximum of 3 segments.</description></item>
    /// </list>
    /// </remarks>
    public record AuditAction
    {
        /// <summary>
        /// The normalized "Category.Operation" string stored in the database.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Returns the raw action string, ideal for serialization and log exports.
        /// </summary>
        public override string ToString() => Value;

        /// <summary>
        /// Facilitates seamless integration with EF Core LINQ queries and string-based logging sinks.
        /// </summary>
        public static implicit operator string(AuditAction action) => action?.Value ?? string.Empty;

        /// <summary>
        /// Internal constructor allows the Persistence layer (ValueConverters) to materialize 
        /// the record from the database without re-triggering the strict Regex validation.
        /// </summary>
        internal AuditAction(string value) => Value = value;

        /// <summary>
        /// Required by Entity Framework Core for materialization and proxy creation.
        /// </summary>
#pragma warning disable CS8618
        protected AuditAction() : base() { }
#pragma warning restore CS8618

        /// <summary>
        /// Flexible factory for creating an <see cref="AuditAction"/> from multiple segments.
        /// Supports hierarchical signatures such as "User.Create" or "User.Email.Update".
        /// </summary>
        /// <param name="segments">A variable list of strings representing the hierarchy (Category, Sub-Category, Operation).</param>
        /// <returns>A validated <see cref="AuditAction"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if requirements for segments, format, or length are not met.</exception>
        public static AuditAction Create(params string[] segments)
        {
            // MINIMUM HIERARCHY CHECK
            // Audit logs must contain at least a 'Category' and an 'Operation' (Noun.Verb) 
            // to be useful for security filtering and reporting.
            if (segments == null || segments.Length < 2)
                throw new ArgumentException("At least a Category and Operation are required.");

            // NORMALIZATION
            // Trims whitespace from each segment and joins them with a period.
            // This ensures consistent search results in the database (e.g., "User . Create" becomes "User.Create").
            var action = string.Join(".", segments.Select(s => s.Trim()));

            // STRUCTURAL VALIDATION
            // Verifies against the ActionRegex to ensure the dot-notation is strictly alphanumeric
            // and stays within the allowed nesting depth (typically 2 to 3 segments).
            if (!ActionRegex.IsMatch(action))
                throw new ArgumentException("Action must be alphanumeric dot-notation (e.g., User.Email.Update).");

            // SCHEMA BOUNDARY CHECK
            // Prevents SQL truncation errors by verifying the final string length against 
            // the centralized database configuration constant.
            if (action.Length > EntityConfigurationConstants.AuditAction.MaxLength)
                throw new ArgumentException($"Action exceeds the maximum allowed length of {EntityConfigurationConstants.AuditAction.MaxLength}.");

            return new AuditAction(action);
        }

        /// <summary>
        /// Structural validator for the hierarchical Audit Action signature.
        /// Enforces a strict alphanumeric dot-notation format (e.g., 'Category.Operation' 
        /// or 'Category.SubCategory.Operation').
        /// </summary>
        /// <remarks>
        /// Pattern Breakdown:
        /// - ^[a-zA-Z0-9]+          : Starts with an alphanumeric Category.
        /// - (\.[a-zA-Z0-9]+)       : Followed by a period and an alphanumeric segment.
        /// - {1,2}$                 : Limits total segments to 2 or 3 (1 or 2 dots).
        /// - RegexOptions.Compiled  : Pre-compiles the pattern for high-performance 
        ///                            throughput during high-frequency logging.
        /// </remarks>
        private static readonly Regex ActionRegex =
            new Regex(@"^[a-zA-Z0-9]+(\.[a-zA-Z0-9]+){1,2}$", RegexOptions.Compiled);
    }
}
