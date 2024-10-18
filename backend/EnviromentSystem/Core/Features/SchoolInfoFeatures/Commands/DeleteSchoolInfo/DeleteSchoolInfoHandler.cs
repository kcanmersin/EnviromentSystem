using MediatR;
using Core.Data;
using Core.Shared;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.DeleteSchoolInfo
{
    public class DeleteSchoolInfoHandler : IRequestHandler<DeleteSchoolInfoCommand, Result<DeleteSchoolInfoResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<DeleteSchoolInfoCommand> _validator;

        public DeleteSchoolInfoHandler(ApplicationDbContext context, IValidator<DeleteSchoolInfoCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<DeleteSchoolInfoResponse>> Handle(DeleteSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<DeleteSchoolInfoResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage)
                );
            }

            var schoolInfo = await _context.SchoolInfos.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (schoolInfo == null)
            {
                return Result.Failure<DeleteSchoolInfoResponse>(new Error("NotFound", "SchoolInfo not found."));
            }

            _context.SchoolInfos.Remove(schoolInfo);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteSchoolInfoResponse { Id = request.Id, Success = true });
        }
    }
}
