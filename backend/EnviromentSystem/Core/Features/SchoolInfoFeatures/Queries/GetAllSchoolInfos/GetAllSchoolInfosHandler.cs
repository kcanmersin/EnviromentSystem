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
                Month = s.Month
            }).ToList();

            var response = new GetAllSchoolInfosResponse
            {
                SchoolInfos = responseItems
            };

            return Result.Success(response);
        }
    }
}