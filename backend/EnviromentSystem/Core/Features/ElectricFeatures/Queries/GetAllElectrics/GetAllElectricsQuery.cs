using Core.Shared;
using MediatR;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectrics
{
    public class GetAllElectricsQuery : IRequest<Result<GetAllElectricsResponse>>
    {
    }
}
