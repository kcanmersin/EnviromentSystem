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
                return Result.Failure<UpdateSchoolInfoResponse>(new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var schoolInfo = await _context.SchoolInfos
                .Include(s => s.Vehicles) // Ensure we include Vehicles in the query
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schoolInfo == null)
            {
                return Result.Failure<UpdateSchoolInfoResponse>(new Error("NotFound", "SchoolInfo not found."));
            }

            schoolInfo.NumberOfPeople = request.NumberOfPeople;
            schoolInfo.Year = request.Year;
            schoolInfo.ModifiedDate = DateTime.UtcNow;

            schoolInfo.Vehicles.CarsManagedByUniversity = request.CarsManagedByUniversity;
            schoolInfo.Vehicles.CarsEnteringUniversity = request.CarsEnteringUniversity;
            schoolInfo.Vehicles.MotorcyclesEnteringUniversity = request.MotorcyclesEnteringUniversity;

            await _context.SaveChangesAsync(cancellationToken);

            var response = new UpdateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Success = true,
                CarsManagedByUniversity = schoolInfo.Vehicles.CarsManagedByUniversity,
                CarsEnteringUniversity = schoolInfo.Vehicles.CarsEnteringUniversity,
                MotorcyclesEnteringUniversity = schoolInfo.Vehicles.MotorcyclesEnteringUniversity,
            };

            return Result.Success(response);
        }
    }
}
