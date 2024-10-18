using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.SchoolInfoFeatures.DeleteSchoolInfo
{
    public class DeleteSchoolInfoHandler : IRequestHandler<DeleteSchoolInfoCommand, DeleteSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;

        public DeleteSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteSchoolInfoResponse> Handle(DeleteSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var schoolInfo = await _context.SchoolInfos.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schoolInfo == null)
            {
                return new DeleteSchoolInfoResponse { Success = false, Message = "SchoolInfo not found." };
            }

            _context.SchoolInfos.Remove(schoolInfo);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteSchoolInfoResponse { Id = request.Id, Success = true };
        }
    }
}
