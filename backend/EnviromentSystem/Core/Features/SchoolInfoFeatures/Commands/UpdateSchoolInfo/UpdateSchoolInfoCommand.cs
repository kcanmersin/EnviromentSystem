using MediatR;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Commands.UpdateSchoolInfo
{
    public class UpdateSchoolInfoCommand : IRequest<Result<UpdateSchoolInfoResponse>>
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; } = string.Empty;
    }
}
