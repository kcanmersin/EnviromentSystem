using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using API.Contracts.SchoolInfo;

namespace Core.Features.SchoolInfoFeatures.Queries.GetSchoolInfo
{
    public class GetSchoolInfoHandler : IRequestHandler<GetSchoolInfoQuery, GetSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetSchoolInfoResponse> Handle(GetSchoolInfoQuery request, CancellationToken cancellationToken)
        {
            var schoolInfo = await _context.SchoolInfos
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schoolInfo == null) return null;

            return new GetSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month
            };
        }
    }
}
