using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.DeleteNaturalGas
{
    public class DeleteNaturalGasValidator : AbstractValidator<DeleteNaturalGasCommand>
    {
        public DeleteNaturalGasValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

}
