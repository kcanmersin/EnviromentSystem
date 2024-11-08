using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectrics
{
    public class GetAllElectricsQuery : IRequest<Result<GetAllElectricsResponse>>
    {
        public Guid? BuildingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
