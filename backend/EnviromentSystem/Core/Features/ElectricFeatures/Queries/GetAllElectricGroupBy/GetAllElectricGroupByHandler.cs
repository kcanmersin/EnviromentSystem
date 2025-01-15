using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectricGroupBy
{
    public class GetAllElectricGroupByHandler : IRequestHandler<GetAllElectricGroupByQuery, Result<GetAllElectricGroupByResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllElectricGroupByHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllElectricGroupByResponse>> Handle(GetAllElectricGroupByQuery request, CancellationToken cancellationToken)
        {
            var electricsQuery = _context.Electrics.AsQueryable();

            if (request.BuildingId.HasValue)
                electricsQuery = electricsQuery.Where(e => e.BuildingId == request.BuildingId.Value);

            if (request.StartDate.HasValue)
                electricsQuery = electricsQuery.Where(e => e.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                electricsQuery = electricsQuery.Where(e => e.Date <= request.EndDate.Value);

            var groupedElectrics = await electricsQuery
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new ElectricGroupByResponse
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalUsage = g.Sum(e => e.Usage)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllElectricGroupByResponse { GroupedElectrics = groupedElectrics });
        }
    }
}