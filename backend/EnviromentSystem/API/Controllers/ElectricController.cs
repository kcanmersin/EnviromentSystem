using MediatR;
using Microsoft.AspNetCore.Mvc;
using Core.Features.ElectricFeatures.Commands.CreateElectric;
using Core.Features.ElectricFeatures.Commands.UpdateElectric;
using Core.Features.ElectricFeatures.Commands.DeleteElectric;
using Core.Features.ElectricFeatures.Queries.GetAllElectrics;
using Core.Features.ElectricFeatures.Queries.GetElectricById;

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
            if (!result.IsSuccess) return BadRequest(result.Error.Message);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateElectricCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return NotFound(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteElectricCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return NotFound(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetElectricByIdQuery() { Id = id };
            var result = await _mediator.Send(query);

            if (result == null) return NotFound("Electric record not found.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllElectricsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
