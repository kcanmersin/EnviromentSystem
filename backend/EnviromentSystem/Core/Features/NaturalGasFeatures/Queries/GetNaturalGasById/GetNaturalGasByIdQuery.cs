using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Queries.GetNaturalGasById
{
    public class GetNaturalGasByIdQuery : IRequest<Result<GetNaturalGasByIdResponse>>
    {
        public Guid Id { get; set; }
    }

}
