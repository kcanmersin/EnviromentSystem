using FluentValidation;

namespace Core.Features.ElectricFeatures.Commands.UpdateElectric
{
    public class UpdateElectricValidator : AbstractValidator<UpdateElectricCommand>
    {
        public UpdateElectricValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.BuildingId).NotEmpty().WithMessage("BuildingId is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");
            RuleFor(x => x.KWHValue).GreaterThanOrEqualTo(0).WithMessage("KWHValue cannot be negative.");
        }
    }
}
