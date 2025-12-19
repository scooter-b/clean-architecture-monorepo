using MediatR;

namespace User.Application.Features.User.Commands.CreateUser
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Guid>
    {

        public CreateUserRequestHandler()
        {
        
        }

        public Task<Guid> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
