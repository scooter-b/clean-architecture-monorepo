using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Core.Data.Converters
{
    /// <summary>
    /// A value converter that enforces UTC storage and retrieval for <see cref="DateTime"/> values.
    /// </summary>
    /// <remarks>
    /// This converter ensures contributor‑safe handling of <see cref="DateTime"/> properties:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// When saving to the database, values are normalized to UTC using <see cref="DateTime.ToUniversalTime"/>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// When reading from the database, values are explicitly marked as <see cref="DateTimeKind.Utc"/>
    /// using <see cref="DateTime.SpecifyKind"/>.
    /// </description>
    /// </item>
    /// </list>
    /// By centralizing this logic, contributors don’t need to remember to manually enforce UTC,
    /// preventing subtle bugs caused by local time zone conversions.
    /// </remarks>
    public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the conversion rules:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <b>To Provider:</b> Converts <see cref="DateTime"/> values to UTC before saving.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <b>From Provider:</b> Ensures retrieved values are explicitly marked as UTC.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public UtcDateTimeConverter() : base(
            v => v.ToUniversalTime(),                  // Normalize before saving
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) // Mark as UTC when reading
        {
        }
    }

}
