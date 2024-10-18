using MediatR;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Commands.DeleteSchoolInfo
{
    public class DeleteSchoolInfoCommand : IRequest<Result<DeleteSchoolInfoResponse>>
    {
        public Guid Id { get; set; }
    }
}
