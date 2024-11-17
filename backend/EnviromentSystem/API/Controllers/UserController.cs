using API.Contracts.User;
using Core.Features.UserFeatures.Register;
using Core.Features.UserFeatures.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                Roles = request.Roles 
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(Register), new { id = result.Value.Id }, new
            {
                Id = result.Value.Id,
                Email = result.Value.Email,
                Name = result.Value.Name,
                Surname = result.Value.Surname
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return Ok(new
            {
                Token = result.Value.Token,
                Email = result.Value.Email,
                Name = result.Value.Name,
                Surname = result.Value.Surname
            });
        }
    }
}
