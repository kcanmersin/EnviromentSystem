using Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using API.Contracts.SchoolInfo;
using Core.Features.SchoolInfoFeatures.Queries.GetAllSchoolInfos;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.GetAllSchoolInfos
{
    public class GetAllSchoolInfosHandler : IRequestHandler<GetAllSchoolInfosQuery, Result<GetAllSchoolInfosResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllSchoolInfosHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllSchoolInfosResponse>> Handle(GetAllSchoolInfosQuery request, CancellationToken cancellationToken)
        {
            var schoolInfos = await _context.SchoolInfos
                .AsNoTracking()
                .Include(s => s.Vehicles)  // Include the Vehicles data
                .ToListAsync(cancellationToken);

            if (!schoolInfos.Any())
            {
                return Result.Failure<GetAllSchoolInfosResponse>(new Error("NotFound", "No school info records found."));
            }

            var responseItems = schoolInfos.Select(s => new GetSchoolInfoResponse
            {
                Id = s.Id,
                NumberOfPeople = s.NumberOfPeople,
                Year = s.Year,
                CarsManagedByUniversity = s.Vehicles?.CarsManagedByUniversity ?? 0,
                CarsEnteringUniversity = s.Vehicles?.CarsEnteringUniversity ?? 0,
                MotorcyclesEnteringUniversity = s.Vehicles?.MotorcyclesEnteringUniversity ?? 0,
            }).ToList();

            var response = new GetAllSchoolInfosResponse
            {
                SchoolInfos = responseItems
            };

            return Result.Success(response);
        }
    }
}
