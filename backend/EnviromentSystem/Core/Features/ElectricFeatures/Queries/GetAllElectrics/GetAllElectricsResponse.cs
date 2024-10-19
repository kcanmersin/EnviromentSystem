using Core.Features.ElectricFeatures.Queries.GetElectricById;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectrics
{
    public class GetAllElectricsResponse
    {
        public List<GetElectricByIdResponse> Electrics { get; set; } = new();
    }
}
