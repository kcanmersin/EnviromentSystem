namespace API.Contracts.SchoolInfo
{
    public class CreateSchoolInfoRequest
    {
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
    }
}
