using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        // Method to train the model
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

        // Method to get predictions from the API
        public async Task<List<PredictionResponse>> GetPredictionAsync(string consumptionType, string buildingId = null, int months = 12)
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

            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response into a custom ResponseWrapper object
            var responseWrapper = JsonSerializer.Deserialize<ResponseWrapper>(responseContent, new JsonSerializerOptions
            {
                Converters =
                {
                    new DateTimeOffsetConverter() // Use the DateTimeOffset converter here
                }
            });

            return responseWrapper?.Predictions ?? new List<PredictionResponse>();
        }
    }

    // Custom DateTimeOffset converter class
    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            return DateTimeOffset.Parse(dateString); // Direct parsing into DateTimeOffset
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:sszzz")); // Custom date format
        }
    }

    // Wrapper for the API response
    public class ResponseWrapper
    {
        public string Message { get; set; }
        public List<PredictionResponse> Predictions { get; set; }
    }

    // PredictionResponse class
}
