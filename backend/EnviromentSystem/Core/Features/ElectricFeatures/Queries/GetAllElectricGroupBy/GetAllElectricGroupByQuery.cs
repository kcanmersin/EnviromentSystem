using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectricGroupBy
{
    public class GetAllElectricGroupByQuery : IRequest<Result<GetAllElectricGroupByResponse>>
    {
        public Guid? BuildingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
