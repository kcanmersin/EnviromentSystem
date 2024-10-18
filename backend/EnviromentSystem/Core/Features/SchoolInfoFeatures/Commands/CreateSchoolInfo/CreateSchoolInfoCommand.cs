using MediatR;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo
{
    public class CreateSchoolInfoCommand : IRequest<Result<CreateSchoolInfoResponse>>
    {
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
    }
}
