using Core.Service.CarbonFootprint;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Features.CarbonFootprint
{
    public interface ICarbonFootprintService
    {
        Task<CarbonFootprintForYearDto> GetCarbonFootprintByYearAsync(int year);
        Task<CarbonFootprintForAllYearsDto> GetCarbonFootprintForAllYearsAsync();
    }
}
