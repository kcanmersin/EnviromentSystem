namespace Core.Features.SchoolInfoFeatures.Commands.DeleteSchoolInfo
{
    public class DeleteSchoolInfoResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
