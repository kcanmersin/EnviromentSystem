namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoResponse
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
