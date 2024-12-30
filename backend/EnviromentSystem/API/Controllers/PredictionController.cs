using Core.Service.PredictionService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost("train")]
        public async Task<IActionResult> TrainModel(
            [FromQuery] string consumptionType = "electric",
            [FromQuery] string buildingId = null)
        {
            try
            {
                var result = await _predictionService.TrainModelAsync(consumptionType, buildingId);
                return Ok(new { Message = "Model trained successfully", Details = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPost("predict")]
        public async Task<IActionResult> GetPrediction(
            [FromQuery] string consumptionType = "electric",
            [FromQuery] string buildingId = null,
            [FromQuery] int months = 12)
        {
            try
            {
                var result = await _predictionService.GetPredictionAsync(consumptionType, buildingId, months);
                return Ok(new { Message = "Prediction fetched successfully", Predictions = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
