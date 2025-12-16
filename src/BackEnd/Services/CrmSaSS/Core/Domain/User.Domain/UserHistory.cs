using Shared.Core.Entities;
using Shared.Core.Entities.Enums;
using User.Domain.Enums;

namespace User.Domain
{
    /// <summary>
    /// The user history entity that records significant events related to a user.
    /// </summary>
    public class UserHistory : BaseEntity
    {
        /// <summary>
        /// This constructor is for EF Core.
        /// </summary>
        protected UserHistory() { }

        /// <summary>
        /// The private constructor to enforce factory usage
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventType"></param>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        public UserHistory(
            Guid userId, UserEventType eventType, ActorType actorType,
            Guid? createdBy = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            EventType = eventType;
            CreatedAt = DateTime.UtcNow;
            CreatedByActorType = actorType;
            CreatedBy = actorType == ActorType.User ? createdBy : null;
        }

        /// <summary>
        /// Foreign key to the associated user.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// The type of event that occurred.
        /// </summary>
        public UserEventType EventType { get; private set; }

        // Navigation properties

        /// <summary>
        /// The user associated with this history record.
        /// </summary>
        public User User { get; set; } = null!;

        // Factory methods

        /// <summary>
        /// The factory method to create a new <see cref="UserHistory"/> instance.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventType"></param>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        /// <returns>A <see cref="UserHistory"/> instance.</returns>
        public static UserHistory Create(
            Guid userId, UserEventType eventType, ActorType actorType,
            Guid? createdBy = null)
        {
            return new UserHistory(userId, eventType, actorType, createdBy);
        }
    }
}
