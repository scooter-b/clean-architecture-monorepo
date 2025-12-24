using Shared.Core.Contracts.Persistence;
using User.Domain.Entities;

namespace User.Application.Contracts.Persistence
{
    public interface IUserHistoryRepository : IGenericRepository<UserHistory>
    {
    }
}
