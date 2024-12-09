namespace API.Contracts.SchoolInfo
{
    public class GetSchoolInfoResponse
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public int CarsManagedByUniversity { get; set; }
        public int CarsEnteringUniversity { get; set; }
        public int MotorcyclesEnteringUniversity { get; set; }
        public int TotalVehiclesEntering { get; set; }
    }
}
