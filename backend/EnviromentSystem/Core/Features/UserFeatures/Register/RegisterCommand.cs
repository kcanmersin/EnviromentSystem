using Core.Shared;
using MediatR;
using System.Collections.Generic;

namespace Core.Features.UserFeatures.Register
{
    public class RegisterCommand : IRequest<Result<RegisterResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
