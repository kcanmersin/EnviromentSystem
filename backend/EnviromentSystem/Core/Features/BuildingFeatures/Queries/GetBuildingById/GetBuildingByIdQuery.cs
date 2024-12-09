using MediatR;
using Core.Shared;
using System;

namespace Core.Features.BuildingFeatures.Queries.GetBuildingById
{
    public class GetBuildingByIdQuery : IRequest<Result<GetBuildingByIdResponse>>
    {
        public Guid Id { get; set; }
    }
}
