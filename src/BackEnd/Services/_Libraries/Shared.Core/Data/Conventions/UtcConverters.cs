using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Core.Data.Conventions
{
    /// <summary>
    /// The UTC converters for DateTime and nullable DateTime.
    /// </summary>
    public static class UtcConverters
    {
        /// <summary>
        /// The converter to ensure DateTime values are treated as UTC.
        /// </summary>
        public static readonly ValueConverter<DateTime, DateTime> UtcDateTimeConverter =
            new(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        /// <summary>
        /// The converter to ensure nullable DateTime values are treated as UTC.
        /// </summary>
        public static readonly ValueConverter<DateTime?, DateTime?> NullableUtcDateTimeConverter =
            new(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null);
    }
}

