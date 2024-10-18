namespace API.Contracts.SchoolInfo
{
    public class UpdateSchoolInfoRequest
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
    }
}
