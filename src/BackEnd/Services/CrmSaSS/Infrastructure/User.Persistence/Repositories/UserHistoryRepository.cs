using Shared.Repositories;
using User.Application.Interfaces;
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
