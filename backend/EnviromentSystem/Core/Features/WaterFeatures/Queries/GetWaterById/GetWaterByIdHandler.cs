using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.WaterFeatures.Queries.GetWaterById
{
    public class GetWaterByIdHandler : IRequestHandler<GetWaterByIdQuery, Result<GetWaterByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetWaterByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetWaterByIdResponse>> Handle(GetWaterByIdQuery request, CancellationToken cancellationToken)
        {
            var water = await _context.Waters
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (water == null)
            {
                return Result.Failure<GetWaterByIdResponse>(
                    new Error("WaterNotFound", "Water record not found."));
            }

            var response = new GetWaterByIdResponse
            {
                Id = water.Id,
                Date = water.Date,
                InitialMeterValue = water.InitialMeterValue,
                FinalMeterValue = water.FinalMeterValue,
                Usage = water.Usage
            };

            return Result.Success(response);
        }
    }
}
