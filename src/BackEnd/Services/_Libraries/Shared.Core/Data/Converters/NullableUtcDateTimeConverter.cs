using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Core.Data.Converters
{
    /// <summary>
    /// A value converter that enforces UTC storage and retrieval for nullable <see cref="DateTime"/> values.
    /// </summary>
    /// <remarks>
    /// This converter ensures contributor‑safe handling of <see cref="DateTime?"/> properties:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// When saving to the database, non‑null values are normalized to UTC using <see cref="DateTime.ToUniversalTime"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// When reading from the database, non‑null values are explicitly marked as <see cref="DateTimeKind.Utc"/>
    /// using <see cref="DateTime.SpecifyKind"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Null values are preserved as null, ensuring optional timestamps remain optional without conversion side effects.
    /// </description>
    /// </item>
    /// </list>
    /// By centralizing this logic, contributors don’t need to remember to manually enforce UTC
    /// or handle null checks themselves, preventing subtle bugs caused by local time zone conversions.
    /// </remarks>
    public class NullableUtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableUtcDateTimeConverter"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the conversion rules:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <b>To Provider:</b> Converts non‑null <see cref="DateTime"/> values to UTC before saving.
        /// Null values are passed through unchanged.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>From Provider:</b> Ensures retrieved non‑null values are explicitly marked as UTC.
        /// Null values are preserved.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public NullableUtcDateTimeConverter() : base(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,                  // Normalize before saving if not null
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v) // Mark as UTC when reading if not null
        {
        }
    }
}
