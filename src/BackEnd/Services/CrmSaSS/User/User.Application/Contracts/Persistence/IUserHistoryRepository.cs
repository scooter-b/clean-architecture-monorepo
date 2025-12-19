using Shared.Core.Contracts.Persistence;

namespace User.Application.Contracts.Persistence
{
    public interface IUserHistoryRepository : IGenericRepository<Domain.UserHistory>
    {
    }
}
