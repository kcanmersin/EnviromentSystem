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
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSchoolInfoCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteSchoolInfoCommand { Id = id };
            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new GetSchoolInfoQuery { Id = id };
            var response = await _mediator.Send(query);

            if (response == null) return NotFound("SchoolInfo not found.");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllSchoolInfosQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }

    }
}
