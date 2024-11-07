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
            var electrics = await _context.Electrics
                .Include(e => e.Building)  
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var responseItems = electrics.Select(e => new GetElectricByIdResponse
            {
                Id = e.Id,
                BuildingId = e.BuildingId,
                BuildingName = e.Building.Name,
                E_MeterCode = e.Building.E_MeterCode,
                Date = e.Date,
                InitialMeterValue = e.InitialMeterValue,
                FinalMeterValue = e.FinalMeterValue,
                Usage = e.Usage,
                KWHValue = e.KWHValue,
                CreatedDate = e.CreatedDate
            }).ToList();

            return Result.Success(new GetAllElectricsResponse { Electrics = responseItems });
        }
    }
}
