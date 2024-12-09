using System;

namespace Core.Features.BuildingFeatures.Queries.GetBuildingById
{
    public class GetBuildingByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string E_MeterCode { get; set; }
        public string G_MeterCode { get; set; }
    }
}
