namespace User.Domain.Enums
{
    /// <summary>
    /// The type of event that has occurred related to a user.
    /// </summary>
    public enum UserEventType
    {
        /// <summary>
        /// Indicates that a previously inactive user account has been re‑enabled and is now active in the system.
        /// </summary>
        UserActivated,

        /// <summary>
        /// Indicates that a user account has been disabled, suspended, or otherwise made inactive, preventing further use.
        /// </summary>
        UserDeactivated,

        /// <summary>
        /// Represents the creation of a new user record in the system — the initial onboarding event.
        /// </summary>
        UserCreated,

        /// <summary>
        /// Signals that an existing user’s details (profile, credentials, or settings) have been modified.
        /// </summary>
        UserUpdated,

        /// <summary>
        /// Denotes that a user account has been permanently removed from the system.
        /// </summary>
        UserDeleted,
    }
}
