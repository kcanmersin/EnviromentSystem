using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.ElectricFeatures.Queries.GetAllElectricGroupBy
{
    public class GetAllElectricGroupByResponse
    {
        public List<ElectricGroupByResponse> GroupedElectrics { get; set; } = new();
    }

}
