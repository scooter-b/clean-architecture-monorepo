using Shared.Core.Abstractions;
using User.Domain.Entities;

namespace User.Domain.Interfaces
{
    /// <summary>
    /// This interface consolidates read and write operations for <see cref="UserAccount"/> entities.
    /// It serves as the primary data contract for User management within the domain.
    /// </summary>
    public interface IUserAccountRepository : IReadWriteRepository<UserAccount>
    {
    }
}
