using MediatR;
using User.Application.Features.User._Share;

namespace User.Application.Features.User.Commands.CreateUser
{
    public record CreateUserRequest(BaseUserRequest request) : IRequest<Guid>;
}
