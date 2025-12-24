using Shared.Core.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Core.Primitives
{
    /// <summary>
    /// Provides the fundamental identity for all domain entities.
    /// Uses UUID v7 for sequential, time-ordered GUIDs to optimize database performance.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        /// <summary>
        /// The unique identifier for the entity. Immutable after creation.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; init; }

        /// <summary>
        /// Parameterless constructor required for Entity Framework Core materialization.
        /// </summary>
        protected BaseEntity() { }

        /// <summary>
        /// Initializes a new instance with a specific ID or a new sequential UUID v7.
        /// </summary>
        /// <param name="id">The explicit ID to use, or null to generate a new sequential ID.</param>
        protected BaseEntity(Guid? id = null)
        {
            Id = id ?? Guid.CreateVersion7();
        }
    }
}
