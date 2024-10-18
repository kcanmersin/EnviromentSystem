using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.SchoolInfoFeatures.UpdateSchoolInfo
{
    public class UpdateSchoolInfoHandler : IRequestHandler<UpdateSchoolInfoCommand, UpdateSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;

        public UpdateSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateSchoolInfoResponse> Handle(UpdateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var schoolInfo = await _context.SchoolInfos.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schoolInfo == null)
            {
                return new UpdateSchoolInfoResponse { Success = false, Message = "SchoolInfo not found." };
            }

            schoolInfo.NumberOfPeople = request.NumberOfPeople;
            schoolInfo.Year = request.Year;
            schoolInfo.Month = request.Month;
            schoolInfo.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month,
                Success = true
            };
        }
    }
}
