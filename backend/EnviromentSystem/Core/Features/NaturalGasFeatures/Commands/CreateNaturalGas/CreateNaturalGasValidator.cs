using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.CreateNaturalGas
{
    public class CreateNaturalGasValidator : AbstractValidator<CreateNaturalGasCommand>
    {
        public CreateNaturalGasValidator()
        {
            RuleFor(x => x.BuildingId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.InitialMeterValue).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FinalMeterValue).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Usage).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SM3Value).GreaterThanOrEqualTo(0);
        }
    }

}
