using Core.Service.PredictionService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnomalyController : ControllerBase
    {
        private readonly IAnomalyService _anomalyService;

        public AnomalyController(IAnomalyService anomalyService)
        {
            _anomalyService = anomalyService;
        }

        [HttpPost("get-anomaly")]
        public async Task<IActionResult> GetAnomaly(
            [FromQuery] string consumptionType = "electric",
            [FromQuery] string buildingId = null,
            [FromQuery] float threshold = 0.05f)
        {
            try
            {
                var result = await _anomalyService.GetAnomalyAsync(consumptionType, buildingId, threshold);
                return Ok(new { Message = "Anomalies fetched successfully", Anomalies = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
