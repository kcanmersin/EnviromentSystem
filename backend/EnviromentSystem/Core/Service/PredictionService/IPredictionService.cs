using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.PredictionService
{
    public interface IPredictionService
    {
        Task<string> TrainModelAsync(string consumptionType, string buildingId = null, int epochs = 50, int batchSize = 16);
        Task<string> GetPredictionAsync(string consumptionType, string buildingId = null, int months = 12);
    }
}
