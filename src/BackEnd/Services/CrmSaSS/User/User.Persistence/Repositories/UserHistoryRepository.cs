using Shared.Repositories;
using User.Application.Contracts.Persistence;
using User.Domain.Entities;
using User.Persistence.Contexts;

namespace User.Persistence.Repositories
{
    public class UserHistoryRepository : GenericRepository<UserHistory>, IUserHistoryRepository
    {
        public UserHistoryRepository(UserDbContext context)
            : base(context)
        {
        }
    }
}
