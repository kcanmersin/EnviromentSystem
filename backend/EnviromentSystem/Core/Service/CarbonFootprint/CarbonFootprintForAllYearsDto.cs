using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.CarbonFootprint
{
    public class CarbonFootprintForAllYearsDto
    {
        public List<CarbonFootprintForYearDto> YearlyFootprints { get; set; }
    }
}
