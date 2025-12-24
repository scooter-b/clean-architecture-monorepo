using Shared.Core.Constants;

namespace Shared.Core.ValueObjects
{
    /// <summary>
    /// Represents a validated and normalized email address.
    /// Enforces length constraints and ensures lowercase normalization for identity consistency.
    /// </summary>
    public record EmailAddress
    {
        /// <summary>
        /// The normalized (lowercase) email address value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The private constructor to enforce the use of the factory method.
        /// </summary>
        /// <param name="value"></param>
        private EmailAddress(string value) => Value = value;

        /// <summary>
        /// Creates a new <see cref="EmailAddress"/> instance after validation and normalization.
        /// </summary>
        /// <param name="email">The raw email string to validate.</param>
        /// <returns>A validated <see cref="EmailAddress"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown if the email is null, empty, invalid, or too long.</exception>
        public static EmailAddress Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            // Normalize immediately for validation and storage
            var normalized = email.Trim().ToLowerInvariant();

            if (!normalized.Contains("@"))
                throw new ArgumentException("Invalid email format.");

            if (normalized.Length > EntityConfigurationConstants.EmailAddress.MaxLength)
                throw new ArgumentException($"Email exceeds maximum allowed length of {EntityConfigurationConstants.EmailAddress.MaxLength}.");

            return new EmailAddress(normalized);
        }

        /// <summary>
        /// Returns the normalized email string.
        /// </summary>
        public override string ToString() => Value;

        /// <summary>
        /// Implicitly converts the <see cref="EmailAddress"/> object to its string representation.
        /// </summary>
        public static implicit operator string(EmailAddress email) => email.Value;
    }
}
