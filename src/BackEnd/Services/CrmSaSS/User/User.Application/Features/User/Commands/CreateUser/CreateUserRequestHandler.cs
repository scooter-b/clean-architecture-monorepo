using MediatR;
using Shared.Core.Abstractions;
using Shared.Core.ValueObjects;
using User.Domain.Entities;
using User.Domain.Interfaces;

namespace User.Application.Features.User.Commands.CreateUser
{
    /// <inheritdoc />
    /// <remarks>
    /// This handler coordinates a multi-stage transaction:
    /// <list type="number">
    /// <item>Verifies email uniqueness via a non-tracking database check.</item>
    /// <item>Initializes the <see cref="UserAccount"/> and <see cref="UserAccountLog"/> entities.</item>
    /// <item>Stages both records and commits them as a single atomic unit via <see cref="IUnitOfWork"/>.</item>
    /// </list>
    /// </remarks>
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Guid>
    {
        private readonly IUserAccountRepository _userAccountRepo;
        private readonly IUserAccountLogRepository _userAuditLogRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserRequestHandler(
            IUserAccountRepository userAccountRepository,
            IUserAccountLogRepository userAccountAuditLogRepository,
            IUnitOfWork unitOfWork)
        {
            _userAccountRepo = userAccountRepository;
            _userAuditLogRepo = userAccountAuditLogRepository;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when a user with the provided email already exists.</exception>
        public async Task<Guid> Handle(CreateUserRequest command, CancellationToken ct)
        {
            // Check for existing user (Optimized check with inherited remarks)
            bool exists = await _userAccountRepo.AnyAsync(r => r.Email == command.Email, ct);

            if (exists)
            {
                throw new InvalidOperationException($"User with email '{command.Email}' already exists.");
            }

            // TODO: fluent validation needed here.


            // TODO: Replace with actual actor ID
            var auditPrincipal = AuditPrincipal.FromUser(Guid.NewGuid());
            
            // Use factory methods to ensure consistent entity state
            var userAccount = UserAccount.Create(
                command.FirstName,
                command.LastName,
                command.Email,
                auditPrincipal);

            var auditLog = UserAccountLog.NewUserRegistration(
                userAccount.Id,
                auditPrincipal);

            // Stage the records in the shared Change Tracker
            await _userAccountRepo.AddAsync(userAccount, ct);
            await _userAuditLogRepo.AddAsync(auditLog, ct);

            // Finalize the Unit of Work transaction
            await _unitOfWork.SaveChangesAsync(ct);

            return userAccount.Id;
        }
    }
}
