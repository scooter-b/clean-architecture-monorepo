using MediatR;

namespace User.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest>
    {
        public UpdateUserRequestHandler()
        {
        }

        public Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
