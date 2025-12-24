using Shared.Core.Entities;
using Shared.Core.Entities.Enums;
using Shared.Core.Entities.Extensions;
using User.Domain.Enums;

namespace User.Domain.Entities
{
    /// <summary>
    /// The user entity representing a system user.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// This constructor is for EF Core.
        /// </summary>
        protected User() { }

        /// <summary>
        /// Private constructor to enforce factory usage
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastNname"></param>
        /// <param name="email"></param>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        private User(
            string firstName, string lastNname, string email,
            ActorType actorType, Guid? createdBy = null)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastNname;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            CreatedByActorType = actorType;
            CreatedBy = actorType.ResolveUserId(createdBy);

        }

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; private set; } = string.Empty;

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; private set; } = string.Empty;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; private set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user is currently active.
        /// </summary>
        public bool IsActive => !DeactivatedAt.HasValue;

        /// <summary>
        /// The date and time when the user was deactivated. Null if the user is active.
        /// </summary>
        public DateTime? DeactivatedAt { get; private set; } = null;

        // Computed properties

        /// <summary>
        /// The full name of the user, combining first and last names.
        /// </summary>
        public string FullName { get; private set; } = default!;

        // Navigation properties

        /// <summary>
        /// The collection of history records associated with the user.
        /// </summary>
        public ICollection<UserHistory> Histories { get; set; } = new List<UserHistory>();

        // Factory methods

        /// <summary>
        /// Factory method to create a new <see cref="User"/> instance.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastNname"></param>
        /// <param name="email"></param>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        /// <returns><see cref="User"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (User user, UserHistory history) Create(
            string firstName, string lastNname, string email,
            ActorType actorType, Guid? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastNname))
                throw new ArgumentException("Last name is required.", nameof(lastNname));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            var user = new User(firstName, lastNname, email, actorType, createdBy);

            var history = UserHistory.Create(
                user.Id,
                UserEventType.UserCreated,
                actorType,
                createdBy);

            return (user, history);
        }

        /// <summary>
        /// The method to update user details.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastNname"></param>
        /// <param name="email"></param>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        /// <returns>A <see cref="UserHistory"/> instance.</returns>
        public UserHistory Update(
            string? firstName, string? lastNname, string? email,
            ActorType actorType, Guid? createdBy = null)
        {
            FirstName = string.IsNullOrWhiteSpace(firstName) ? FirstName : firstName;
            LastName = string.IsNullOrWhiteSpace(lastNname) ? LastName : lastNname;
            Email = string.IsNullOrWhiteSpace(email) ? Email : email;

            UpdatedAt = DateTime.UtcNow;
            UpdatedByActorType = actorType;
            UpdatedBy = actorType.ResolveUserId(createdBy);

            return UserHistory.Create(
                Id,
                UserEventType.UserUpdated,
                actorType,
                createdBy);
        }

        /// <summary>
        /// The method to delete the user.
        /// </summary>
        /// <param name="actorType"></param>
        /// <param name="createdBy"></param>
        /// <returns>A <see cref="UserHistory"/> instance.</returns>
        public UserHistory Delete(
            ActorType actorType, Guid? createdBy = null)
        {
            Deactivate(actorType, createdBy);

            DeletedAt = DateTime.UtcNow;
            DeletedByActorType = actorType;
            DeletedBy = actorType.ResolveUserId(createdBy);

            return UserHistory.Create(
                Id,
                UserEventType.UserDeleted,
                actorType,
                createdBy);
        }

        /// <summary>
        /// Deactivates the user account if it is currently active.
        /// </summary>
        /// <remarks>
        /// - If the user is already inactive, the method exits without making changes.
        /// - Sets the <see cref="DeactivatedAt"/> property to the current UTC timestamp,
        ///   marking the exact time the account was deactivated.
        /// </remarks>
        /// <returns>A <see cref="UserHistory"/> instance if deactivation occurred; otherwise, null.</returns>
        public UserHistory? Deactivate(
            ActorType actorType, Guid? createdBy = null)
        {
            if (!IsActive)
            {
                return null; // User is already inactive, no action needed
            }

            UpdatedAt = DateTime.UtcNow;
            UpdatedByActorType = actorType;
            UpdatedBy = actorType.ResolveUserId(createdBy);

            DeactivatedAt = DateTime.UtcNow; // Record the deactivation time

            return UserHistory.Create(
                Id,
                UserEventType.UserDeactivated,
                actorType,
                createdBy);
        }

        /// <summary>
        /// Re-activates the user account if it is currently inactive.
        /// </summary>
        /// <remarks>
        /// - If the user is already active, the method exits without changes.
        /// - Clears the <see cref="DeactivatedAt"/> timestamp to indicate the user is active again.
        /// </remarks>
        /// <returns>A <see cref="UserHistory"/> instance if activation occurred; otherwise, null.</returns>
        public UserHistory? Activate(
            ActorType actorType, Guid? createdBy = null)
        {
            if (IsActive)
            {
                return null; // User is already active, no action needed
            }

            UpdatedAt = DateTime.UtcNow;
            UpdatedByActorType = actorType;
            UpdatedBy = actorType.ResolveUserId(createdBy);
            
            DeactivatedAt = null; // Reset deactivation date to mark as active

            return UserHistory.Create(
                Id,
                UserEventType.UserActivated,
                actorType,
                createdBy);
        }
    }
}
