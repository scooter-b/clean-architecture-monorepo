namespace Shared.Core.Constants
{
    /// <summary>
    /// A centralized registry of standardized Nouns (Categories) and Verbs (Operations).
    /// This ensures consistent naming across the "Category.Operation" audit hierarchy.
    /// </summary>
    public static class AuditEvents
    {
        /// <summary>
        /// High-level Resource Categories (The Nouns).
        /// Represents the primary domain object or boundary being acted upon.
        /// </summary>
        public static class Categories
        {
            /// <summary>
            /// Actions related to the UserAccount Identity (e.g., "UserAccount.Create", "UserAccount.Delete").
            /// Typically used for lifecycle management of the UserAccount record itself.
            /// </summary>
            public const string UserAccount = "UserAccount";

            /// <summary>
            /// Actions related to the User's system account state (e.g., "Account.Lock", "Account.Unlock").
            /// Focused on authorization, access levels, and subscription status.
            /// </summary>
            public const string Account = "Account";

            /// <summary>
            /// High-sensitivity actions involving credentials or MFA (e.g., "Security.Login", "Security.PasswordChange").
            /// Used to isolate events for security monitoring and threat detection alerts.
            /// </summary>
            public const string Security = "Security";

            /// <summary>
            /// Actions involving personal metadata or preferences (e.g., "Profile.Update", "Profile.PhotoChange").
            /// Distinguishes routine data changes from core account or security modifications.
            /// </summary>
            public const string Profile = "Profile";
        }

        /// <summary>
        /// Granular resource attributes or sub-modules (The Middle Segment / Attributes).
        /// Used for specific tracking of sensitive data changes.
        /// </summary>
        public static class SubCategories
        {
            /// <summary>
            /// Actions specifically involving the onboarding or sign-up process.
            /// Distinguishes new account creation from internal system-driven "User.Create" events.
            /// </summary>
            public const string Registration = "Registration";

            /// <summary>
            /// Actions specifically targeting the primary or secondary Email addresses.
            /// High-sensitivity: Changes here often trigger security notifications.
            /// </summary>
            public const string Email = "Email";

            /// <summary>
            /// Actions involving credential modifications. 
            /// Critical for tracking password resets and security breaches.
            /// </summary>
            public const string Password = "Password";

            /// <summary>
            /// Actions involving Multi-Factor Authentication (e.g., "Security.Mfa.Enable").
            /// Essential for auditing identity assurance levels.
            /// </summary>
            public const string Mfa = "Mfa";

            /// <summary>
            /// Actions related to authorization, permissions, or group memberships.
            /// Primary target for auditing privilege escalation.
            /// </summary>
            public const string Role = "Role";

            /// <summary>
            /// Actions involving state transitions (e.g., "Account.Status.Locked").
            /// Tracks the lifecycle and availability of the resource.
            /// </summary>
            public const string Status = "Status";

            /// <summary>
            /// Actions involving general metadata, preferences, or PII not covered by other sub-categories.
            /// </summary>
            public const string Profile = "Profile";
        }

        /// <summary>
        /// Standardized Operations (The Verbs).
        /// Represents the specific action taken. Following 2025 industry standards, 
        /// these use present-tense PascalCase for optimal log indexing and searchability.
        /// </summary>
        public static class Operations
        {
            /// <summary>
            /// Represents the initialization or insertion of a new resource.
            /// </summary>
            public const string Create = "Create";

            /// <summary>
            /// Represents a generic modification to an existing resource.
            /// </summary>
            public const string Update = "Update";

            /// <summary>
            /// Represents the removal or soft-deletion of a resource from the system.
            /// </summary>
            public const string Delete = "Delete";

            /// <summary>
            /// Tracks authentication entry events; critical for security and session auditing.
            /// </summary>
            public const string Login = "Login";

            /// <summary>
            /// Tracks the intentional termination of a user session.
            /// </summary>
            public const string Logout = "Logout";

            /// <summary>
            /// Represents an administrative or security-driven restriction of access (e.g., account lockout).
            /// </summary>
            public const string Lock = "Lock";

            /// <summary>
            /// Represents the restoration of access to a previously restricted resource.
            /// </summary>
            public const string Unlock = "Unlock";

            /// <summary>
            /// Represents a specialized update specifically for sensitive state transitions 
            /// (e.g., "Password.Change" or "Permission.Change").
            /// </summary>
            public const string Change = "Change";

        }
    }


}
