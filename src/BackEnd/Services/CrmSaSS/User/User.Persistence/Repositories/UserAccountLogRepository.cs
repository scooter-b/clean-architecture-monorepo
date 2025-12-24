using Microsoft.EntityFrameworkCore;
using Shared.Core.Persistence;
using User.Domain.Entities;
using User.Domain.Interfaces;
using User.Persistence.Contexts;

namespace User.Persistence.Repositories
{
    /// <remarks>
    /// This repository is specialized for <see cref="UserAccountLog"/> records.
    /// <para>
    /// <b>Performance Note:</b> Since audit logs are append-only and high-volume, 
    /// this repository focuses on Write operations. Ensure <c>UnitOfWork.SaveChangesAsync()</c> 
    /// is called to persist log entries alongside their parent transactions.
    /// </para>
    /// </remarks>
    public class UserAccountLogRepository : ReadWriteRepository<UserAccountLog>, IUserAccountLogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountLogRepository"/> class.
        /// </summary>
        /// <param name="context">The <see cref="BaseDbContext"/> shared with other repositories to ensure transactional integrity.</param>
        public UserAccountLogRepository(UserDbContext context) : base(context)
        {
        }
    }
}
