namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoResponse
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }

        // Vehicle-related properties
        public int CarsManagedByUniversity { get; set; }
        public int CarsEnteringUniversity { get; set; }
        public int MotorcyclesEnteringUniversity { get; set; }
        public int TotalVehiclesEntering { get; set; }
    }
}
