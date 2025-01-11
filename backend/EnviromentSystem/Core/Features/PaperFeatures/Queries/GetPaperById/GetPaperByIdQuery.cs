using Core.Features.WaterFeatures.Queries.GetWaterById;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Queries.GetPaperById
{
    public class GetPaperByIdQuery : IRequest<Result<GetPaperByIdResponse>>
    {
        public Guid Id { get; set; }
    }
}
