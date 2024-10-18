using MediatR;

namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoCommand : IRequest<CreateSchoolInfoResponse>
    {
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
    }
}
