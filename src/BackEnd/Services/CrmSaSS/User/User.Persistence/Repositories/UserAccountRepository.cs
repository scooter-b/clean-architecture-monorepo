using Microsoft.EntityFrameworkCore;
using Shared.Core.Persistence;
using User.Domain.Entities;
using User.Domain.Interfaces;
using User.Persistence.Contexts;

namespace User.Persistence.Repositories
{
    /// <remarks>
    /// Implementation uses the application's shared <see cref="DbContext"/>. 
    /// All changes are tracked locally until the Unit of Work is committed.
    /// </remarks>
    public class UserAccountRepository : ReadWriteRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(UserDbContext context) : base(context)
        {
        }

    }
}
