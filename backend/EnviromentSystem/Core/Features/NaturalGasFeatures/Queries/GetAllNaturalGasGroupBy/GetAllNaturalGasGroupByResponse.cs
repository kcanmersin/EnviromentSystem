using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGasGroupBy
{
    public class GetAllNaturalGasGroupByResponse
    {
        public List<NaturalGasGroupByResponse> GroupedNaturalGas { get; set; } = new();
    }

}
