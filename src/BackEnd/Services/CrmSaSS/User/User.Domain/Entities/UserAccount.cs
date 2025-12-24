using Shared.Core.Constants;
using Shared.Core.Primitives;
using Shared.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Domain.Entities
{
    /// <summary>
    /// Represents a primary system user account.
    /// Inherits from <see cref="AuditEntity"/> to ensure all identity changes 
    /// are tracked with high-integrity metadata and sequential UUID v7 identifiers.
    /// </summary>
    public class UserAccount : AuditEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccount"/> class with the specified first name, last name,
        /// email address, and creator information.
        /// </summary>
        private UserAccount(
            PersonName firstName, PersonName lastName, EmailAddress email, AuditPrincipal auditPrincipal)
            : base(auditPrincipal)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        /// <summary>
        /// The parameterless constructor is required by Entity Framework for materialization.
        /// </summary>
        private UserAccount() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


        // Mapped properties

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [Required]
        [MaxLength(EntityConfigurationConstants.PersonName.MaxLength)]
        public PersonName FirstName { get; private set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [Required]
        [MaxLength(EntityConfigurationConstants.PersonName.MaxLength)]
        public PersonName LastName { get; private set; }

        // TODO: Add Unique Index on Email in DbContext configuration
        // TODO: Consider normalization (e.g., lowercase)
        // TODO: Consider rate limiting for email queries to prevent abuse
        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(EntityConfigurationConstants.EmailAddress.MaxLength)]
        public EmailAddress Email { get; private set; }

        /// <summary>
        /// Staging area for a new email that hasn't been verified yet.
        /// </summary>
        [EmailAddress]
        [MaxLength(EntityConfigurationConstants.EmailAddress.MaxLength)]
        public EmailAddress? PendingEmail { get; private set; }

        /// <summary>
        /// Stores the previous login email for security recovery if an account is hijacked.
        /// </summary>
        [EmailAddress]
        [MaxLength(EntityConfigurationConstants.EmailAddress.MaxLength)]
        public EmailAddress? PreviousEmail { get; private set; }

        /// <summary>
        /// The date and time when the user was deactivated. Null if the user is active.
        /// </summary>
        public DateTime? DeactivatedAt { get; private set; } = null;

        /// <summary>
        /// The date and time of the user's last login.
        /// </summary>
        public DateTime? LastLoginAt { get; private set; } = null;

        // Computed properties

        // TODO: need dbcontext configuration
        /// <summary>
        /// The full name of the user, combining first and last names.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; private set; } = default!;


        // Non-Mapped properties

        /// <summary>
        /// Indicates whether the user is currently active.
        /// </summary>
        [NotMapped]
        public bool IsActive => !DeactivatedAt.HasValue;

        /// <summary>
        /// The inverted name of the user in "LastName, FirstName" format.
        /// </summary>
        [NotMapped]
        public string InvertedName => $"{LastName}, {FirstName}";


        // Factory methods

        /// <summary>
        /// The factory method to create a new <see cref="UserAccount"/> instance.
        /// </summary>
        /// <param name="firstName">The user's first name. Cannot be null, empty, or consist only of white-space characters.</param>
        /// <param name="lastName">The user's last name. Cannot be null, empty, or consist only of white-space characters.</param>
        /// <param name="email">The user's email address. Cannot be null, empty, or consist only of white-space characters.</param>
        /// <param name="createdBy">The identifier of the user or process that created this account.</param>
        /// <returns><see cref="UserAccount"/></returns>
        /// <exception cref="ArgumentException"><paramref name="firstName"/>, <paramref name="lastName"/>, or <paramref name="email"/> is null, empty, or
        /// consists only of white-space characters.</exception>
        public static UserAccount Create(
            string firstName, string lastName, string email, AuditPrincipal auditPrincipal)
        {
            // Return the validated instance
            return new UserAccount(
                PersonName.Create(firstName),
                PersonName.Create(lastName),
                EmailAddress.Create(email),
                auditPrincipal);
        }


        // Methods

        /// <summary>
        /// The method to update the first and last name.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="updatedBy"></param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateName(string firstName, string lastName, AuditPrincipal auditPrincipal)
        {
            // Idempotency: If the change is already pending or matches current names
            if (FirstName == firstName && LastName == lastName) return;

            // Update
            FirstName = PersonName.Create(firstName);
            LastName = PersonName.Create(lastName);

            // Audit
            SetModified(auditPrincipal);
        }

        /// <summary>
        /// The method to deactivate the user account.
        /// </summary>
        public void Deactivate(AuditPrincipal auditPrincipal)
        {
            // TODO: may need to pass in who is performing the deactivation
            if (DeactivatedAt.HasValue)
            {
                // Update
                DeactivatedAt = DateTime.UtcNow;

                // Audit
                SetModified(auditPrincipal);
            }
        }

        /// <summary>
        /// The method to reactivate the user account.
        /// </summary>
        public void Reactivate(AuditPrincipal auditPrincipal)
        {           
            // Update
            DeactivatedAt = null;

            // Audit
            SetModified(auditPrincipal);
        }

        /// <summary>
        /// Stages a new email address for verification. 
        /// Does not overwrite the current Email until confirmed.
        /// </summary>
        /// <param name="newEmail">The new email string to be validated and staged.</param>
        /// <param name="requestedBy">The identity of the user initiating the change.</param>
        public void InitiateEmailChange(string newEmail, AuditPrincipal auditPrincipal)
        {
            // Convert raw input into a validated Value Object
            // This automatically handles Trim, ToLowerInvariant, and Length checks.
            var emailVo = EmailAddress.Create(newEmail);

            // Idempotency Guard
            // If the new email matches the current primary email or the already-pending one,
            // we exit silently to avoid redundant audit logs or database writes.
            if (Email == emailVo || PendingEmail == emailVo)
                return;

            // Staging the change
            // Note: PendingEmail is now a Value Object type, not a raw string.
            PendingEmail = emailVo;

            // Update the standard audit metadata (Base class method)
            // This method now accepts the 'string' and validates it against AuditPrincipal internally.
            SetModified(auditPrincipal);
        }


        /// <summary>
        /// Finalizes a staged email change, archives the previous identity, 
        /// and records the transition in the audit log.
        /// </summary>
        public void ConfirmEmailChange(AuditPrincipal auditPrincipal)
        {
            // Guard against invalid state
            if (PendingEmail is null)
                throw new InvalidOperationException("No pending email change exists for this account.");

            // Archive current identity for security recovery
            PreviousEmail = Email;

            //// TODO: Create the Identity Audit Log entry
            //// This is part of the same transaction when SaveChanges is called
            //var log = new IdentityAuditLog(
            //    userAccountId: Id,
            //    action: "EmailChange",
            //    oldValue: Email.Value,
            //    newValue: PendingEmail.Value,
            //    changedBy: confirmedBy
            //);

            //_identityHistory.Add(log);

            // Promote pending to primary and clear staging
            Email = PendingEmail;
            PendingEmail = null;

            // Audit
            SetModified(auditPrincipal);

            // TODO: Identity Provider Integration (2025 Standard)
            // In Clean Architecture, we usually raise a Domain Event here.
            // The Application Layer listens to this event to update the Identity Provider (e.g., Auth0, Entra ID).
            //AddDomainEvent(new UserEmailChangedEvent(Id, Email));
        }
    }
}
