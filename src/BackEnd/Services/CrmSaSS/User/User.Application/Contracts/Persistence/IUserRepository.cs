using Shared.Core.Contracts.Persistence;

namespace User.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<Domain.Entities.User>
    {
    }
}
