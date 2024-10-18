using MediatR;
using API.Contracts.SchoolInfo;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.Queries.GetSchoolInfo
{
    public class GetSchoolInfoQuery : IRequest<Result<GetSchoolInfoResponse>>
    {
        public Guid Id { get; set; }
    }
}
