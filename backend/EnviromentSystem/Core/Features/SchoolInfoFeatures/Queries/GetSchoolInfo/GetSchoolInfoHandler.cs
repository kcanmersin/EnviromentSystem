using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using API.Contracts.SchoolInfo;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Queries.GetSchoolInfo
{
    public class GetSchoolInfoHandler : IRequestHandler<GetSchoolInfoQuery, Result<GetSchoolInfoResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetSchoolInfoResponse>> Handle(GetSchoolInfoQuery request, CancellationToken cancellationToken)
        {
            var schoolInfo = await _context.SchoolInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schoolInfo == null)
            {
                return Result.Failure<GetSchoolInfoResponse>(new Error("NotFound", "SchoolInfo not found."));
            }

            var response = new GetSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month
            };

            return Result.Success(response);
        }
    }
}
