using Core.Shared;
using MediatR;

namespace Core.Features.UserFeatures.Login
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
