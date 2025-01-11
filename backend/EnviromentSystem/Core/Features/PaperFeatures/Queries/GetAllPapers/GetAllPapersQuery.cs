using Core.Features.WaterFeatures.Queries.GetAllWaters;
using Core.Shared;
using MediatR;

namespace Core.Features.PaperFeatures.Queries.GetAllPapers
{
    public class GetAllPapersQuery : IRequest<Result<GetAllPapersResponse>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
