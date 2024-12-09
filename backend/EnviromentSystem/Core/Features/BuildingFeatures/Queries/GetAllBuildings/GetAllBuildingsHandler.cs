using Core.Data;
using Core.Features.BuildingFeatures.Queries.GetAllBuildings;
using Core.Features.BuildingFeatures.Queries.GetBuildingById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.BuildingFeatures.Queries.GetAllBuildings
{
    public class GetAllBuildingsHandler : IRequestHandler<GetAllBuildingsQuery, Result<GetAllBuildingsResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllBuildingsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllBuildingsResponse>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            var buildingsQuery = _context.Buildings.AsQueryable();

            // Apply the name filter only if it's provided
            if (!string.IsNullOrEmpty(request.NameFilter))
            {
                buildingsQuery = buildingsQuery.Where(b => b.Name.Contains(request.NameFilter));
            }

            var buildings = await buildingsQuery
                .Select(b => new GetBuildingByIdResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    E_MeterCode = b.E_MeterCode,
                    G_MeterCode = b.G_MeterCode
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new GetAllBuildingsResponse { Buildings = buildings });
        }
    }
}
