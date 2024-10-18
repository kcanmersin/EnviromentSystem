using MediatR;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.UpdateSchoolInfo
{
    public class UpdateSchoolInfoHandler : IRequestHandler<UpdateSchoolInfoCommand, UpdateSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UpdateSchoolInfoCommand> _validator;

        public UpdateSchoolInfoHandler(ApplicationDbContext context, IValidator<UpdateSchoolInfoCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<UpdateSchoolInfoResponse> Handle(UpdateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
