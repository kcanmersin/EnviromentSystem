using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.DeleteSchoolInfo
{
    public class DeleteSchoolInfoHandler : IRequestHandler<DeleteSchoolInfoCommand, DeleteSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<DeleteSchoolInfoCommand> _validator;

        public DeleteSchoolInfoHandler(ApplicationDbContext context, IValidator<DeleteSchoolInfoCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<DeleteSchoolInfoResponse> Handle(DeleteSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
