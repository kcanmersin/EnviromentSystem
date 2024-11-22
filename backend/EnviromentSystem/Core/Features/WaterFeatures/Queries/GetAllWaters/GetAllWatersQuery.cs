using Core.Shared;
using MediatR;

namespace Core.Features.WaterFeatures.Queries.GetAllWaters
{
    public class GetAllWatersQuery : IRequest<Result<GetAllWatersResponse>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
