using MediatR;
using API.Contracts.SchoolInfo;
using Core.Features.SchoolInfoFeatures.Queries.GetAllSchoolInfos;
using Core.Shared;

namespace Core.Features.SchoolInfoFeatures.GetAllSchoolInfos
{
    public class GetAllSchoolInfosQuery : IRequest<Result<GetAllSchoolInfosResponse>>
    {
    }
}
