using MediatR;
using Microsoft.AspNetCore.Mvc;
using Core.Features.ElectricFeatures.Commands.CreateElectric;
using Core.Features.ElectricFeatures.Commands.UpdateElectric;
using Core.Features.ElectricFeatures.Commands.DeleteElectric;
using Core.Features.ElectricFeatures.Queries.GetAllElectrics;
using Core.Features.ElectricFeatures.Queries.GetElectricById;
using API.Contracts.Electric;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElectricController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ElectricController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateElectricCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateElectricCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Error = "Mismatched ID in request URL and payload." });

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return result.Error.Code == "NotFound" ? NotFound(new { Error = result.Error.Message }) : BadRequest(new { Error = result.Error.Message });

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromBody] DeleteElectricRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { Error = "Mismatched ID in request URL and payload." });

            var command = new DeleteElectricCommand { Id = request.Id };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return result.Error.Code == "NotFound" ? NotFound(new { Error = result.Error.Message }) : BadRequest(new { Error = result.Error.Message });

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetElectricByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(new { Error = result.Error.Message });

            return Ok(result.Value);
        }

        [HttpGet]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? buildingId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new GetAllElectricsQuery
            {
                BuildingId = buildingId,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(new { Error = result.Error.Message });
        }
    }
}
