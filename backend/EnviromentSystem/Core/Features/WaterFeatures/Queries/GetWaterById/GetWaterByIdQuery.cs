using Core.Shared;
using MediatR;

namespace Core.Features.WaterFeatures.Queries.GetWaterById
{
    public class GetWaterByIdQuery : IRequest<Result<GetWaterByIdResponse>>
    {
        public Guid Id { get; set; }
    }
}
