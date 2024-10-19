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
            var electric = await _context.Electrics.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (electric == null)
            {
                return Result.Failure<GetElectricByIdResponse>(
                    new Error("ElectricNotFound", "Electric record not found."));
            }

            var response = new GetElectricByIdResponse
            {
                Id = electric.Id,
                SchoolInfoId = electric.SchoolInfoId,
                Consumption = electric.Consumption,
                Cost = electric.Cost,
                //Year = electric.Year,
                //Month = electric.Month,
                CreatedDate = electric.CreatedDate
            };

            return Result.Success(response);
        }
    }
}
