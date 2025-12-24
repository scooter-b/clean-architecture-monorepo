using MediatR;

namespace User.Application.Features.User.Commands.CreateUser
{
    public record CreateUserRequest(
        string FirstName,
        string LastName,
        string Email) : IRequest<Guid>;
}
