using Core.Data;
using Core.Features.PaperFeatures.Queries.GetPaperById;
using Core.Features.WaterFeatures.Queries.GetAllWaters;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.PaperFeatures.Queries.GetAllPapers
{
    public class GetAllPapersHandler : IRequestHandler<GetAllPapersQuery, Result<GetAllPapersResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllPapersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllPapersResponse>> Handle(GetAllPapersQuery request, CancellationToken cancellationToken)
        {
            var papersQuery = _context.Papers.AsQueryable();

            if (request.StartDate.HasValue)
                papersQuery = papersQuery.Where(p => p.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                papersQuery = papersQuery.Where(p => p.Date <= request.EndDate.Value);

            var papers = await papersQuery
                .Select(p => new GetPaperByIdResponse
                {
                    Id = p.Id,
                    Date = p.Date,
                    Usage = p.Usage
                })
            .ToListAsync(cancellationToken);

            return Result.Success(new GetAllPapersResponse { Papers = papers });
        }
    }
}
