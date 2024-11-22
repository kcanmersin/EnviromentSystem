using Core.Data;
using Core.Features.WaterFeatures.Queries.GetWaterById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.WaterFeatures.Queries.GetAllWaters
{
    public class GetAllWatersHandler : IRequestHandler<GetAllWatersQuery, Result<GetAllWatersResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllWatersHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllWatersResponse>> Handle(GetAllWatersQuery request, CancellationToken cancellationToken)
        {
            var watersQuery = _context.Waters.AsQueryable();

            if (request.StartDate.HasValue)
                watersQuery = watersQuery.Where(w => w.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                watersQuery = watersQuery.Where(w => w.Date <= request.EndDate.Value);

            var waters = await watersQuery
                .Select(w => new GetWaterByIdResponse
                {
                    Id = w.Id,
                    Date = w.Date,
                    InitialMeterValue = w.InitialMeterValue,
                    FinalMeterValue = w.FinalMeterValue,
                    Usage = w.Usage
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllWatersResponse { Waters = waters });
        }
    }
}