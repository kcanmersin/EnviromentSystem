using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Core.Service.PredictionService
{
    public class PredictionService : IPredictionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;

        public PredictionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseApiUrl = configuration["PredictionApi:BaseUrl"];
        }

        public async Task<string> TrainModelAsync(string consumptionType, string buildingId = null, int epochs = 50, int batchSize = 16)
        {
            var requestBody = new
            {
                consumption_type = consumptionType,
                building_id = buildingId,
                epochs,
                batch_size = batchSize
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseApiUrl}/train_model", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to train model: {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPredictionAsync(string consumptionType, string buildingId = null, int months = 12)
        {
            var requestBody = new
            {
                consumption_type = consumptionType,
                building_id = buildingId,
                months
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseApiUrl}/get_prediction", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get predictions: {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
