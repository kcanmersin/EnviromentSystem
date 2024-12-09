using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Core.Service.PredictionService
{
    public class AnomalyService : IAnomalyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public AnomalyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseApiUrl = configuration["PredictionApi:BaseUrl"];
        }

        public async Task<string> GetAnomalyAsync(string consumptionType, string buildingId = null, float threshold = 0.05f)
        {
            var requestBody = new
            {
                consumption_type = consumptionType,
                building_id = buildingId,
                threshold
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseApiUrl}/get_anomaly", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get anomalies: {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
