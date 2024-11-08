using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Core.Shared;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Core.Features.NaturalGasFeatures.Queries.GetNaturalGasById;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGas
{
    public class GetAllNaturalGasHandler : IRequestHandler<GetAllNaturalGasQuery, Result<GetAllNaturalGasResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllNaturalGasHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllNaturalGasResponse>> Handle(GetAllNaturalGasQuery request, CancellationToken cancellationToken)
        {
            var naturalGasQuery = _context.NaturalGasUsages.AsQueryable();

            if (request.BuildingId.HasValue)
                naturalGasQuery = naturalGasQuery.Where(n => n.BuildingId == request.BuildingId.Value);

            if (request.StartDate.HasValue)
                naturalGasQuery = naturalGasQuery.Where(n => n.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                naturalGasQuery = naturalGasQuery.Where(n => n.Date <= request.EndDate.Value);

            var naturalGas = await naturalGasQuery
                .Include(n => n.Building)
                .Select(n => new GetNaturalGasByIdResponse
                {
                    Id = n.Id,
                    BuildingId = n.BuildingId,
                    BuildingName = n.Building.Name,
                    Date = n.Date,
                    InitialMeterValue = n.InitialMeterValue,
                    FinalMeterValue = n.FinalMeterValue,
                    Usage = n.Usage,
                    SM3Value = n.SM3Value,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllNaturalGasResponse { NaturalGas = naturalGas });
        }
    }
}
