using Shared.Core.Abstractions;
using User.Domain.Entities;

namespace User.Domain.Interfaces
{
    public interface IUserAccountLogRepository : IWriteRepository<UserAccountLog>
    {
    }
}
