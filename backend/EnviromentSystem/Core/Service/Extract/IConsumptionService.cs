using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Service.Extract
{
    public interface IConsumptionService
    {
        Task<List<ConsumptionDataDto>> GetConsumptionDataAsync(string consumptionType);
    }
}
