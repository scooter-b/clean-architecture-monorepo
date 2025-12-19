using MediatR;
using User.Application.Features.User._Share;

namespace User.Application.Features.User.Commands.UpdateUser
{
    public record UpdateUserRequest(Guid UserId, BaseUserRequest request) : IRequest;
}
