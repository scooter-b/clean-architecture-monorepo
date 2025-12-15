namespace Shared.Entities
{
    /// <summary>
    /// The base entity class that includes common properties for all entities.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// The unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The timestamp when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The identifier of the user who created the entity.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The timestamp when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The identifier of the user who last updated the entity.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// The timestamp when the entity was deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// The identifier of the user who deleted the entity.
        /// </summary>
        public Guid? DeletedBy { get; set; }
    }
}
