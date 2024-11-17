using Core.Data.Entity.User;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Features.UserFeatures.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager; 
        private readonly IValidator<RegisterCommand> _validator;

        public RegisterHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IValidator<RegisterCommand> validator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<RegisterResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result.Failure<RegisterResponse>(
                    new Error("UserCreationFailed", string.Join("; ", errors)));
            }

            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new AppRole { Name = role });
                }

                await _userManager.AddToRoleAsync(user, role);
            }

            return Result.Success(new RegisterResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            });
        }
    }
}
