using MediatR;
using API.Contracts.SchoolInfo;

namespace Core.Features.SchoolInfoFeatures.Queries.GetSchoolInfo
{
    public class GetSchoolInfoQuery : IRequest<GetSchoolInfoResponse>
    {
        public Guid Id { get; set; }
    }
}
