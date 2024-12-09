using Core.Service.AnomalyService;
using System.Threading.Tasks;

namespace Core.Service.PredictionService
{
    public interface IAnomalyService
    {
        Task<AnomalyResponse> GetAnomalyAsync(string consumptionType, string buildingId = null, float threshold = 0.05f);
    }
}
