using Core.Data;
using Core.Features.WaterFeatures.Queries.GetWaterById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.PaperFeatures.Queries.GetPaperById
{
    public class GetPaperByIdHandler : IRequestHandler<GetPaperByIdQuery, Result<GetPaperByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetPaperByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<GetPaperByIdResponse>> Handle(GetPaperByIdQuery request, CancellationToken cancellationToken)
        {
            var paper = await _context.Papers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (paper == null)
            {
                return Result.Failure<GetPaperByIdResponse>(
                    new Error("PaperNotFound", "Paper record not found."));
            }

            var response = new GetPaperByIdResponse
            {
                Id = paper.Id,
                Date = paper.Date,
                Usage = paper.Usage
            };

            return Result.Success(response);
        }
    }
}
