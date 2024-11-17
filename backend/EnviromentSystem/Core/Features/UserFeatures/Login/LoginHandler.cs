using Core.Data.Entity.User;
using Core.Service.JWT;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Features.UserFeatures.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IValidator<LoginCommand> _validator;

        public LoginHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService, IValidator<LoginCommand> validator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _validator = validator;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<LoginResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<LoginResponse>(new Error("InvalidCredentials", "Invalid email or password."));
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                return Result.Failure<LoginResponse>(new Error("InvalidCredentials", "Invalid email or password."));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user.Email, user.Id, roles);

            return Result.Success(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            });
        }

    }
}
