using MediatR;
using Core.Data;
using Core.Shared;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.UpdateSchoolInfo
{
    public class UpdateSchoolInfoHandler : IRequestHandler<UpdateSchoolInfoCommand, Result<UpdateSchoolInfoResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateSchoolInfoCommand> _validator;

        public UpdateSchoolInfoHandler(ApplicationDbContext context, IValidator<UpdateSchoolInfoCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<UpdateSchoolInfoResponse>> Handle(UpdateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<UpdateSchoolInfoResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage)
                );
            }

            var schoolInfo = await _context.SchoolInfos.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (schoolInfo == null)
            {
                return Result.Failure<UpdateSchoolInfoResponse>(new Error("NotFound", "SchoolInfo not found."));
            }

            schoolInfo.NumberOfPeople = request.NumberOfPeople;
            schoolInfo.Year = request.Year;
            schoolInfo.Month = request.Month;
            schoolInfo.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            var response = new UpdateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month,
                Success = true
            };

            return Result.Success(response);
        }
    }
}
