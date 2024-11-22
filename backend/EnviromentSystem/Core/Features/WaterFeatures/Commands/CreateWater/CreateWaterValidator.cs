using FluentValidation;

namespace Core.Features.WaterFeatures.Commands.CreateWater
{
    public class CreateWaterValidator : AbstractValidator<CreateWaterCommand>
    {
        public CreateWaterValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");
        }
    }
}
