namespace Core.Features.SchoolInfoFeatures.UpdateSchoolInfo
{
    public class UpdateSchoolInfoResponse
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
