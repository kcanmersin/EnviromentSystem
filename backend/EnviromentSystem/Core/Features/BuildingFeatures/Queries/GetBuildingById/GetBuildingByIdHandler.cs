using Core.Data;
using Core.Features.BuildingFeatures.Queries.GetBuildingById;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.BuildingFeatures.Queries.GetBuildingById
{
    public class GetBuildingByIdHandler : IRequestHandler<GetBuildingByIdQuery, Result<GetBuildingByIdResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetBuildingByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetBuildingByIdResponse>> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (building == null)
            {
                return Result.Failure<GetBuildingByIdResponse>(
                    new Error("BuildingNotFound", "Building not found."));
            }

            var response = new GetBuildingByIdResponse
            {
                Id = building.Id,
                Name = building.Name,
                E_MeterCode = building.E_MeterCode,
                G_MeterCode = building.G_MeterCode
            };

            return Result.Success(response);
        }
    }
}
