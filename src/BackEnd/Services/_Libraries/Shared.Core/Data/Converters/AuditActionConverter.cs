using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Core.ValueObjects;

namespace Shared.Core.Data.Converters
{
    /// <summary>
    /// Defines the database mapping for the <see cref="AuditAction"/> Value Object.
    /// Converts the structured dot-notation record into a standard string column.
    /// </summary>
    public class AuditActionConverter : ValueConverter<AuditAction, string>
    {
        /// <summary>
        /// Initializes the converter for use in EF Core model configuration.
        /// </summary>
        public AuditActionConverter()
            : base(
                // TO DATABASE: Persist the raw "Category.Operation" string
                action => action.Value,

                // FROM DATABASE: Reconstitute the record using the internal constructor
                // This bypasses the Regex validation to prevent legacy data from crashing queries.
                value => new AuditAction(value)
            )
        {
        }
    }
}
