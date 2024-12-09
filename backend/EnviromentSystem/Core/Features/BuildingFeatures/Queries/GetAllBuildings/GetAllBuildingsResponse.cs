using Core.Features.BuildingFeatures.Queries.GetBuildingById;
using System.Collections.Generic;

namespace Core.Features.BuildingFeatures.Queries.GetAllBuildings
{
    public class GetAllBuildingsResponse
    {
        public List<GetBuildingByIdResponse> Buildings { get; set; } = new List<GetBuildingByIdResponse>();
    }
}
