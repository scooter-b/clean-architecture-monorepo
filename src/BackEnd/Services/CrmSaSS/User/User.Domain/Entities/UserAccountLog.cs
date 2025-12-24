using Shared.Core.Constants;
using Shared.Core.Primitives;
using Shared.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace User.Domain.Entities
{
    /// <summary>
    /// This entity provides a specialized audit trail for user account lifecycle events.
    /// It inherits from <see cref="AuditLogEntity"/> to handle standardized action and value tracking.
    /// </summary>
    public class UserAccountLog : AuditLogEntity
    {
        /// <summary>
        /// The unique identifier of the user account associated with this audit log entry.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the target account.</value>
        [Required]
        public Guid UserAccountId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountLog"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor is private to enforce the use of static factory methods for consistent log entry creation.
        /// </remarks>
        private UserAccountLog(
            Guid userAccountId, AuditAction action, AuditPrincipal auditPrincipal, string? oldValue = null, string? newValue = null)
            : base(action, auditPrincipal, oldValue, newValue)
        {
            UserAccountId = userAccountId;
        }

        /// <summary>
        /// The parameterless constructor is required by Entity Framework for materialization.
        /// </summary>
        private UserAccountLog() { }

        /// <summary>
        /// Creates a log entry representing a new user registration event.
        /// </summary>
        /// <param name="userAccountId">The ID of the newly created account.</param>
        /// <param name="performedBy">The identifier of the actor (system or admin) performing the registration.</param>
        /// <returns>A configured <see cref="UserAccountLog"/> instance.</returns>
        public static UserAccountLog NewUserRegistration(
            Guid userAccountId, AuditPrincipal performedBy)
        {
            return new UserAccountLog(
                userAccountId,
                AuditAction.Create(
                    AuditEvents.Categories.UserAccount,
                    AuditEvents.SubCategories.Registration,
                    AuditEvents.Operations.Create),
                performedBy);
        }

        /// <summary>
        /// Creates a log entry representing modifications to an existing user account.
        /// </summary>
        /// <param name="oldValue">The state of the account before the change (usually as a JSON string).</param>
        /// <param name="newValue">The state of the account after the change (usually as a JSON string).</param>
        /// <remarks>
        /// Captured changes typically include security profile updates or contact information changes.
        /// </remarks>
        public static UserAccountLog UpdateUserAccount(
            Guid userAccountId,
            AuditPrincipal performedBy,
            string? oldValue = null,
            string? newValue = null)
        {
            // TODO: may want to allow the specification of more granular action types in the future.
            // for instance, distinguishing between "PasswordChange" vs "ProfileUpdate",etc.
            
            return new UserAccountLog(
                userAccountId,
                AuditAction.Create(
                    AuditEvents.Categories.UserAccount,
                    AuditEvents.Operations.Update),
                performedBy,
                oldValue,
                newValue);
        }
    }
}
