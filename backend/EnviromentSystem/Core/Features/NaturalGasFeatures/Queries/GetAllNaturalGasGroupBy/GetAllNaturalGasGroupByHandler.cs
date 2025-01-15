using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGasGroupBy
{
    public class GetAllNaturalGasGroupByHandler : IRequestHandler<GetAllNaturalGasGroupByQuery, Result<GetAllNaturalGasGroupByResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllNaturalGasGroupByHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllNaturalGasGroupByResponse>> Handle(GetAllNaturalGasGroupByQuery request, CancellationToken cancellationToken)
        {
            var naturalGasQuery = _context.NaturalGasUsages.AsQueryable();

            if (request.BuildingId.HasValue)
                naturalGasQuery = naturalGasQuery.Where(ng => ng.BuildingId == request.BuildingId.Value);

            if (request.StartDate.HasValue)
                naturalGasQuery = naturalGasQuery.Where(ng => ng.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                naturalGasQuery = naturalGasQuery.Where(ng => ng.Date <= request.EndDate.Value);

            var groupedNaturalGas = await naturalGasQuery
                .GroupBy(ng => new { ng.Date.Year, ng.Date.Month })
                .Select(g => new NaturalGasGroupByResponse
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalUsage = g.Sum(ng => ng.Usage)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllNaturalGasGroupByResponse { GroupedNaturalGas = groupedNaturalGas });
        }
    }
}
