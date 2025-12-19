using Shared.Repositories;
using User.Application.Contracts.Persistence;
using User.Persistence.Contexts;

namespace User.Persistence.Repositories
{
    public class UserHistoryRepository : GenericRepository<Domain.UserHistory>, IUserHistoryRepository
    {
        public UserHistoryRepository(UserDbContext context)
            : base(context)
        {
        }
    }
}
