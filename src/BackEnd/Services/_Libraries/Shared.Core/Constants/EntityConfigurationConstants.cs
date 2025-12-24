namespace Shared.Core.Constants
{
    /// <summary>
    /// Provides a centralized location for database schema constraints and 
    /// domain validation limits across the entire solution.
    /// </summary>
    /// <remarks>
    /// Use these constants in Data Annotations, Fluent API configurations, 
    /// and Value Object factory methods to ensure system-wide consistency.
    /// </remarks>
    public static class EntityConfigurationConstants
    {
        /// <summary>
        /// Configuration for electronic mail address fields.
        /// </summary>
        public static class EmailAddress
        {
            /// <summary>
            /// Maximum length for email addresses as per RFC 5321 and industry standards.
            /// </summary>
            public const int MaxLength = 256;
        }

        /// <summary>
        /// Configuration for human name fields (First, Last, Middle).
        /// </summary>
        public static class PersonName
        {
            /// <summary>
            /// Maximum length for a person's name components. 
            /// Aligned with modern directory standards for international support.
            /// </summary>
            public const int MaxLength = 64;
        }

        /// <summary>
        /// Defines schema-level constants for the <see cref="ValueObjects.AuditPrincipal"/>.
        /// </summary>
        public static class AuditPrincipal
        {
            /// <summary>
            /// Maximum length for the identifier of the principal (user or system) 
            /// performing an action. Standardized to 256 to support emails and JWT sub claims.
            /// </summary>
            public const int MaxLength = 256;
        }

        /// <summary>
        /// Defines schema-level constants for the <see cref="ValueObjects.AuditAction"/>.
        /// </summary>
        public static class AuditAction
        {
            /// <summary>
            /// The maximum allowable length for the structured audit event name. 
            /// This limit accommodates the "Category.Operation" hierarchy (e.g., "IdentityManagement.MultiFactorAuthReset"),
            /// ensuring the combined string remains within database indexing limits while supporting
            /// descriptive, machine-readable event names.
            /// </summary>
            public const int MaxLength = 256;
        }

    }

}
