using Core.Features.CarbonFootprint;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarbonFootprintController : ControllerBase
    {
        private readonly ICarbonFootprintService _carbonFootprintService;

        public CarbonFootprintController(ICarbonFootprintService carbonFootprintService)
        {
            _carbonFootprintService = carbonFootprintService;
        }

        // GET: api/CarbonFootprint/all
        [HttpGet("all")]
        public async Task<IActionResult> GetCarbonFootprintForAllYears()
        {
            var carbonFootprintData = await _carbonFootprintService.GetCarbonFootprintForAllYearsAsync();

            if (carbonFootprintData == null || carbonFootprintData.YearlyFootprints.Count == 0)
            {
                return NotFound("No carbon footprint data available.");
            }

            return Ok(carbonFootprintData);
        }

        // GET: api/CarbonFootprint/{year}
        [HttpGet("{year}")]
        public async Task<IActionResult> GetCarbonFootprintByYear(int year)
        {
            var carbonFootprint = await _carbonFootprintService.GetCarbonFootprintByYearAsync(year);

            if (carbonFootprint == null)
            {
                return NotFound($"No carbon footprint data found for the year {year}.");
            }

            return Ok(carbonFootprint);
        }
    }
}
