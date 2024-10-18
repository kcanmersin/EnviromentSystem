using MediatR;
using Core.Data;
using Core.Data.Entity;
using Core.Features.SchoolInfoFeatures.CreateSchoolInfo;

namespace Core.Features.SchoolInfoFeatures.CreateSchoolInfo
{
    public class CreateSchoolInfoHandler : IRequestHandler<CreateSchoolInfoCommand, CreateSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateSchoolInfoResponse> Handle(CreateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var schoolInfo = new SchoolInfo
            {
                NumberOfPeople = request.NumberOfPeople,
                Year = request.Year,
                Month = request.Month,
                CreatedDate = DateTime.UtcNow
            };

            _context.SchoolInfos.Add(schoolInfo);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month,
                CreatedDate = schoolInfo.CreatedDate
            };

            return response;
        }
    }
}
