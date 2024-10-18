using Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using API.Contracts.SchoolInfo;
using Core.Features.SchoolInfoFeatures.Queries.GetAllSchoolInfos;

namespace Core.Features.SchoolInfoFeatures.GetAllSchoolInfos
{
    public class GetAllSchoolInfosHandler : IRequestHandler<GetAllSchoolInfosQuery, GetAllSchoolInfosResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetAllSchoolInfosHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllSchoolInfosResponse> Handle(GetAllSchoolInfosQuery request, CancellationToken cancellationToken)
        {
            var schoolInfos = await _context.SchoolInfos.ToListAsync(cancellationToken);

            var responseItems = schoolInfos.Select(s => new GetSchoolInfoResponse
            {
                Id = s.Id,
                NumberOfPeople = s.NumberOfPeople,
                Year = s.Year,
                Month = s.Month
            }).ToList();

            return new GetAllSchoolInfosResponse(responseItems);
        }
    }
}
