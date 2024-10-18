using MediatR;
using System;

namespace Core.Features.SchoolInfoFeatures.DeleteSchoolInfo
{
    public class DeleteSchoolInfoCommand : IRequest<DeleteSchoolInfoResponse>
    {
        public Guid Id { get; set; }
    }
}
