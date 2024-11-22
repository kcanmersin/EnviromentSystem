using Core.Features.WaterFeatures.Queries.GetWaterById;

namespace Core.Features.WaterFeatures.Queries.GetAllWaters
{
    public class GetAllWatersResponse
    {
        public List<GetWaterByIdResponse> Waters { get; set; } = new();
    }
}
