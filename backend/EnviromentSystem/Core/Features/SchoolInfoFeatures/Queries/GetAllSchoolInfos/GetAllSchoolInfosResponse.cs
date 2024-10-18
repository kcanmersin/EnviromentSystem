using API.Contracts.SchoolInfo;
using System.Collections.Generic;

namespace Core.Features.SchoolInfoFeatures.Queries.GetAllSchoolInfos
{
    public class GetAllSchoolInfosResponse
    {
        public List<GetSchoolInfoResponse> SchoolInfos { get; set; }

        public GetAllSchoolInfosResponse(List<GetSchoolInfoResponse> schoolInfos)
        {
            SchoolInfos = schoolInfos;
        }
    }
}
