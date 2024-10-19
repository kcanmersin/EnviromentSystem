using FluentValidation;

namespace Core.Features.ElectricFeatures.Commands.DeleteElectric
{
    public class DeleteElectricValidator : AbstractValidator<DeleteElectricCommand>
    {
        public DeleteElectricValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
        }
    }
}
