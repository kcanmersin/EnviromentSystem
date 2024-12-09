using MediatR;
using Core.Shared;
using System;

namespace Core.Features.BuildingFeatures.Queries.GetAllBuildings
{
    public class GetAllBuildingsQuery : IRequest<Result<GetAllBuildingsResponse>>
    {
        public string NameFilter { get; set; }
    }
}
