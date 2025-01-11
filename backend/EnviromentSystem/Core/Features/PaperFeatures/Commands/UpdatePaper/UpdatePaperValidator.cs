using FluentValidation;

namespace Core.Features.PaperFeatures.Commands.UpdatePaper
{
    public class UpdatePaperValidator : AbstractValidator<UpdatePaperCommand>
    {
        public UpdatePaperValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.Usage)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Usage cannot be negative.");
        }
    }
}
