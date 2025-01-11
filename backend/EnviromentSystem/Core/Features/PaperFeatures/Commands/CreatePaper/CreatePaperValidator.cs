using FluentValidation;

namespace Core.Features.PaperFeatures.Commands.CreatePaper
{
    public class CreatePaperValidator : AbstractValidator<CreatePaperCommand>
    {
        public CreatePaperValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.Usage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Usage cannot be negative.");
        }
    }
}
