using MediatR;
using API.Contracts.SchoolInfo;
using Core.Features.SchoolInfoFeatures.Queries.GetAllSchoolInfos;

namespace Core.Features.SchoolInfoFeatures.GetAllSchoolInfos
{
    public class GetAllSchoolInfosQuery : IRequest<GetAllSchoolInfosResponse>
    {
    }
}
