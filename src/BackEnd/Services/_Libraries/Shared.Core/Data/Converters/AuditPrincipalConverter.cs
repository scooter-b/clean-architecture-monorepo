using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Core.ValueObjects;

namespace Shared.Core.Data.Converters
{
    /// <summary>
    /// Defines how the <see cref="AuditPrincipal"/> Value Object is persisted in the database.
    /// This allows the domain-specific record to be stored as a standard string column.
    /// </summary>
    public class AuditPrincipalConverter : ValueConverter<AuditPrincipal, string>
    {
        /// <summary>
        /// Initializes a new instance of the converter.
        /// This is used by EF Core during the Model Building process.
        /// </summary>
        public AuditPrincipalConverter()
            : base(
                // TO DATABASE:
                // Extracts the underlying string value for persistence.
                // This ensures the DB only sees a primitive type (NVARCHAR).
                p => p.Value,

                // FROM DATABASE:
                // Uses the static Factory Method to reconstitute the object.
                // This is critical because it triggers your Domain Validation logic
                // (null checks, length checks) even when data is loaded from the DB.
                v => new AuditPrincipal(v)
            )
        {
        }
    }
}
