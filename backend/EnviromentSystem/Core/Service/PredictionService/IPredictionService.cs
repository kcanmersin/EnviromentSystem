using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.PredictionService
{
    public interface IPredictionService
    {
        Task<string> TrainModelAsync(string consumptionType, string buildingId = null);
        Task<UsagePredictionResponse> GetPredictionAsync(string consumptionType, string buildingId = null, int months = 12);
    }
}
