using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Queries.GetNaturalGasById
{
    public class GetNaturalGasByIdHandler : IRequestHandler<GetNaturalGasByIdQuery, Result<GetNaturalGasByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetNaturalGasByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetNaturalGasByIdResponse>> Handle(GetNaturalGasByIdQuery request, CancellationToken cancellationToken)
        {
            var naturalGas = await _context.NaturalGasUsages
                .Include(ng => ng.Building)
                .FirstOrDefaultAsync(ng => ng.Id == request.Id, cancellationToken);

            if (naturalGas == null)
                return Result.Failure<GetNaturalGasByIdResponse>(new Error("NotFound", "Natural gas usage not found."));

            return Result.Success(new GetNaturalGasByIdResponse
            {
                Id = naturalGas.Id,
                BuildingId = naturalGas.BuildingId,
                BuildingName = naturalGas.Building.Name,
                Date = naturalGas.Date,
                InitialMeterValue = naturalGas.InitialMeterValue,
                FinalMeterValue = naturalGas.FinalMeterValue,
                Usage = naturalGas.Usage,
                SM3Value = naturalGas.SM3Value
            });
        }
    }

}
