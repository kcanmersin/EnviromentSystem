using MediatR;
using Core.Data;
using Core.Data.Entity;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoHandler : IRequestHandler<CreateSchoolInfoCommand, Result<CreateSchoolInfoResponse>>
    {
        private readonly ApplicationDbContext _context;

        public CreateSchoolInfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CreateSchoolInfoResponse>> Handle(CreateSchoolInfoCommand request, CancellationToken cancellationToken)
        {
            var campusVehicleEntry = new CampusVehicleEntry
            {
                CarsManagedByUniversity = request.CarsManagedByUniversity,
                CarsEnteringUniversity = request.CarsEnteringUniversity,
                MotorcyclesEnteringUniversity = request.MotorcyclesEnteringUniversity
            };

            var schoolInfo = new SchoolInfo
            {
                NumberOfPeople = request.NumberOfPeople,
                Year = request.Year,
                Vehicles = campusVehicleEntry,
                CreatedDate = DateTime.UtcNow
            };

            _context.SchoolInfos.Add(schoolInfo);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                CreatedDate = schoolInfo.CreatedDate,
                CarsManagedByUniversity = campusVehicleEntry.CarsManagedByUniversity,
                CarsEnteringUniversity = campusVehicleEntry.CarsEnteringUniversity,
                MotorcyclesEnteringUniversity = campusVehicleEntry.MotorcyclesEnteringUniversity,
            };

            return Result.Success(response);
        }
    }
}
