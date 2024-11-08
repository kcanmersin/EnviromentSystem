using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.UpdateNaturalGas
{
    public class UpdateNaturalGasValidator : AbstractValidator<UpdateNaturalGasCommand>
    {
        public UpdateNaturalGasValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.BuildingId).NotEmpty().WithMessage("BuildingId is required.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0).WithMessage("InitialMeterValue cannot be negative.");
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0).WithMessage("FinalMeterValue cannot be negative.");
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0).WithMessage("Usage cannot be negative.");
            RuleFor(x => x.SM3Value).GreaterThanOrEqualTo(0).WithMessage("SM3Value cannot be negative.");
        }
    }

}
