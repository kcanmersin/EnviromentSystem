using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGasGroupBy
{
    public class GetAllNaturalGasGroupByQuery : IRequest<Result<GetAllNaturalGasGroupByResponse>>
    {
        public Guid? BuildingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
