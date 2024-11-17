using FluentValidation;

namespace Core.Features.UserFeatures.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email is required and must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Surname is required.")
                .MaximumLength(50)
                .WithMessage("Surname must not exceed 50 characters.");

            RuleFor(x => x.Roles)
                .Must(roles => roles != null && roles.Any())
                .WithMessage("At least one role must be specified.");
        }
    }
}
