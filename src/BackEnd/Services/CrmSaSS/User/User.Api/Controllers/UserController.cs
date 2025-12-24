using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Api.Models;
using User.Application.Features.User._Share;
using User.Application.Features.User.Commands.CreateUser;
using User.Application.Features.User.Commands.UpdateUser;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// The constructor for UserController.
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// The endpoint to create a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var command = new CreateUserRequest(
                request.FirstName,
                request.LastName,
                request.Email);

            var userId = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateUser), new { id = userId }, null);
        }

        /// <summary>
        /// The endpoint to update an existing user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequestDto request)
        {
            var command = new UpdateUserRequest(
                id,
                new BaseUserRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                });

            await _mediator.Send(command);
            return NoContent();
        }

        // TODO: add delete and get endpoints as necessary
    }
}
