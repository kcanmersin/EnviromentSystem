using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Core.Features.ElectricFeatures.Queries.GetElectricById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectrics
{
    public class GetAllElectricsHandler : IRequestHandler<GetAllElectricsQuery, Result<GetAllElectricsResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllElectricsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllElectricsResponse>> Handle(GetAllElectricsQuery request, CancellationToken cancellationToken)
        {
            var electricsQuery = _context.Electrics.AsQueryable();

            if (request.BuildingId.HasValue)
                electricsQuery = electricsQuery.Where(e => e.BuildingId == request.BuildingId.Value);

            if (request.StartDate.HasValue)
                electricsQuery = electricsQuery.Where(e => e.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                electricsQuery = electricsQuery.Where(e => e.Date <= request.EndDate.Value);

            var electrics = await electricsQuery
                .Include(e => e.Building)
                .Select(e => new GetElectricByIdResponse
                {
                    Id = e.Id,
                    BuildingId = e.BuildingId,
                    BuildingName = e.Building.Name,
                    Date = e.Date,
                    E_MeterCode = e.Building.E_MeterCode,
                    InitialMeterValue = e.InitialMeterValue,
                    FinalMeterValue = e.FinalMeterValue,
                    Usage = e.Usage,
                    KWHValue = e.KWHValue,
                    CreatedDate = e.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllElectricsResponse { Electrics = electrics });
        }
    }
}
