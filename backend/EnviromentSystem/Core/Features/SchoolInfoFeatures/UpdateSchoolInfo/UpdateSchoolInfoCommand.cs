using MediatR;
using System;

namespace Core.Features.SchoolInfoFeatures.UpdateSchoolInfo
{
    public class UpdateSchoolInfoCommand : IRequest<UpdateSchoolInfoResponse>
    {
        public Guid Id { get; set; }
        public int NumberOfPeople { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
    }
}
