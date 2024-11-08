using System;
using MediatR;
using Core.Shared;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGas
{
    public class GetAllNaturalGasQuery : IRequest<Result<GetAllNaturalGasResponse>>
    {
        public Guid? BuildingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
