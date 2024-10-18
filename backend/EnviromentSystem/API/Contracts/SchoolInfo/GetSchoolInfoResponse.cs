namespace API.Contracts.SchoolInfo
{
    public class GetSchoolInfoResponse
    {
        public Guid Id { get; set; } 
        public int NumberOfPeople { get; set; } 
        public int Year { get; set; } 
        public string Month { get; set; } 
    }
}
