using MediatR;
using Core.Data;
using Core.Data.Entity;
using FluentValidation;

namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoHandler : IRequestHandler<CreateSchoolInfoCommand, CreateSchoolInfoResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateSchoolInfoCommand> _validator;

        public CreateSchoolInfoHandler(ApplicationDbContext context, IValidator<CreateSchoolInfoCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CreateSchoolInfoResponse> Handle(CreateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var schoolInfo = new SchoolInfo
            {
                NumberOfPeople = request.NumberOfPeople,
                Year = request.Year,
                Month = request.Month,
                CreatedDate = DateTime.UtcNow
            };

            _context.SchoolInfos.Add(schoolInfo);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month,
                CreatedDate = schoolInfo.CreatedDate
            };
        }
    }
}
