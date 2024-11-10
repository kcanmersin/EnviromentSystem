using Microsoft.AspNetCore.Mvc;
using MediatR;
using API.Contracts.NaturalGas;
using System.Threading.Tasks;
using Core.Features.NaturalGasFeatures.Commands.CreateNaturalGas;
using Core.Features.NaturalGasFeatures.Commands.DeleteNaturalGas;
using Core.Features.NaturalGasFeatures.Commands.UpdateNaturalGas;
using Core.Features.NaturalGasFeatures.Queries.GetAllNaturalGas;
using Core.Features.NaturalGasFeatures.Queries.GetNaturalGasById;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NaturalGasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NaturalGasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNaturalGasCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { Error = result.Error.Message });

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNaturalGasCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Error = "Mismatched ID in request URL and payload." });

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return result.Error.Code == "NotFound" ? NotFound(new { Error = result.Error.Message }) : BadRequest(new { Error = result.Error.Message });

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromBody] DeleteNaturalGasRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { Error = "Mismatched ID in request URL and payload." });

            var command = new DeleteNaturalGasCommand { Id = request.Id };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return result.Error.Code == "NotFound" ? NotFound(new { Error = result.Error.Message }) : BadRequest(new { Error = result.Error.Message });

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetNaturalGasByIdQuery { Id = id };
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
            var query = new GetAllNaturalGasQuery
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
