using Shared.Repositories;
using User.Application.Contracts.Persistence;
using User.Persistence.Contexts;

namespace User.Persistence.Repositories
{
    public class UserRepository : GenericRepository<Domain.User>, IUserRepository
    {
        public UserRepository(UserDbContext context)
            : base(context)
        {
        }
    }
}
