using Core.Features.PaperFeatures.Queries.GetPaperById;

namespace Core.Features.PaperFeatures.Queries.GetAllPapers
{
    public class GetAllPapersResponse
    {
        public List<GetPaperByIdResponse> Papers { get; set; } = new();
    }
}
