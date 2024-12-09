using Core.Features.BuildingFeatures.Queries.GetBuildingById;
using Core.Features.BuildingFeatures.Queries.GetAllBuildings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MediatR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuildingById(Guid id)
        {
            var query = new GetBuildingByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return NotFound(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuildings([FromQuery] string nameFilter = null)
        {
            var query = new GetAllBuildingsQuery { NameFilter = nameFilter };
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

    }
}
