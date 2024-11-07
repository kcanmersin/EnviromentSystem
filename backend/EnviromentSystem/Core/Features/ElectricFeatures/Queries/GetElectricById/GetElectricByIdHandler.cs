using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ElectricFeatures.Queries.GetElectricById
{
    public class GetElectricByIdHandler : IRequestHandler<GetElectricByIdQuery, Result<GetElectricByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetElectricByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetElectricByIdResponse>> Handle(GetElectricByIdQuery request, CancellationToken cancellationToken)
        {
            var electric = await _context.Electrics
                .Include(e => e.Building)  
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (electric == null)
            {
                return Result.Failure<GetElectricByIdResponse>(
                    new Error("ElectricNotFound", "Electric record not found."));
            }

            var response = new GetElectricByIdResponse
            {
                Id = electric.Id,
                BuildingId = electric.BuildingId,
                BuildingName = electric.Building.Name,
                E_MeterCode = electric.Building.E_MeterCode,
                Date = electric.Date,
                InitialMeterValue = electric.InitialMeterValue,
                FinalMeterValue = electric.FinalMeterValue,
                Usage = electric.Usage,
                KWHValue = electric.KWHValue,
                CreatedDate = electric.CreatedDate
            };

            return Result.Success(response);
        }
    }
}
