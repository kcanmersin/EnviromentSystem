using MediatR;
using Microsoft.AspNetCore.Mvc;
using Core.Features.SchoolInfoFeatures.Commands.CreateSchoolInfo;
using Core.Features.SchoolInfoFeatures.Commands.UpdateSchoolInfo;
using Core.Features.SchoolInfoFeatures.Commands.DeleteSchoolInfo;
using Core.Features.SchoolInfoFeatures.Queries.GetSchoolInfo;
using Core.Features.SchoolInfoFeatures.GetAllSchoolInfos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchoolInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSchoolInfoCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error.Message);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSchoolInfoCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return NotFound(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteSchoolInfoCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return NotFound(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetSchoolInfoQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null) return NotFound("SchoolInfo not found.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSchoolInfosQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
