using Shared.Core.Constants;
using System.Globalization;

namespace Shared.Core.ValueObjects
{
    /// <summary>
    /// Represents a validated and formatted person's name (e.g., First or Last name).
    /// Enforces length constraints and applies Title Case formatting.
    /// </summary>
    public record PersonName
    {
        /// <summary>
        /// The formatted name value.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// The private constructor to enforce the use of the factory method.
        /// </summary>
        /// <param name="value"></param>
        private PersonName(string value) => Value = value;

        /// <summary>
        /// Creates a new <see cref="PersonName"/> instance after validation and formatting.
        /// </summary>
        /// <param name="name">The raw name string.</param>
        /// <returns>A validated <see cref="PersonName"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown if the name is empty or exceeds the max length.</exception>
        public static PersonName Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            var trimmed = name.Trim();

            if (trimmed.Length > EntityConfigurationConstants.PersonName.MaxLength)
                throw new ArgumentException($"Name cannot exceed {EntityConfigurationConstants.PersonName.MaxLength} characters.");

            // Title Casing ensures "mcclain" becomes "Mcclain" and "SMITH" becomes "Smith"
            var formatted = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(trimmed.ToLower());

            return new PersonName(formatted);
        }

        /// <summary>
        /// The string representation of the <see cref="PersonName"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value;

        /// <summary>
        /// Allows the <see cref="PersonName"/> to be used as a string without explicit conversion.
        /// </summary>
        public static implicit operator string(PersonName name) => name.Value;
    }
}
