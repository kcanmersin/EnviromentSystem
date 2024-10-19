using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Core.Features.ElectricFeatures.Queries.GetAllElectrics;
using Core.Features.ElectricFeatures.Queries.GetElectricById;

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
            var electrics = await _context.Electrics.ToListAsync(cancellationToken);

            var responseItems = electrics.Select(e => new GetElectricByIdResponse
            {
                Id = e.Id,
                SchoolInfoId = e.SchoolInfoId,
                Consumption = e.Consumption,
                Cost = e.Cost,
                //Year = e.Year,
                //Month = e.Month,
                CreatedDate = e.CreatedDate
            }).ToList();

            return Result.Success(new GetAllElectricsResponse { Electrics = responseItems });
        }
    }
}
