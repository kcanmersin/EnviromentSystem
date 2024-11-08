using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Commands.DeleteNaturalGas
{
    public class DeleteNaturalGasCommand : IRequest<Result<DeleteNaturalGasResponse>>
    {
        public Guid Id { get; set; }
    }

}
