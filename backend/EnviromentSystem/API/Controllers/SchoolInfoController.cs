using MediatR;
using Microsoft.AspNetCore.Mvc;
using Core.Features.SchoolInfoFeatures.CreateSchoolInfo;
using Core.Features.SchoolInfoFeatures.UpdateSchoolInfo;
using Core.Features.SchoolInfoFeatures.DeleteSchoolInfo;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using API.Contracts.SchoolInfo;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolInfoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public SchoolInfoController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
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
            if (!response.Success)
                return NotFound(new UpdateSchoolInfoResponse { Success = false, Message = response.Message });

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteSchoolInfoCommand { Id = id };
            var response = await _mediator.Send(command);
            if (!response.Success)
                return NotFound(new DeleteSchoolInfoResponse { Success = false, Message = response.Message });

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var schoolInfo = await _context.SchoolInfos.FirstOrDefaultAsync(s => s.Id == id);

            if (schoolInfo == null)
                return NotFound(new { Message = "SchoolInfo not found." });

            var response = new GetSchoolInfoResponse
            {
                Id = schoolInfo.Id,
                NumberOfPeople = schoolInfo.NumberOfPeople,
                Year = schoolInfo.Year,
                Month = schoolInfo.Month
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schoolInfos = await _context.SchoolInfos.ToListAsync();

            var response = schoolInfos.Select(s => new GetSchoolInfoResponse
            {
                Id = s.Id,
                NumberOfPeople = s.NumberOfPeople,
                Year = s.Year,
                Month = s.Month
            }).ToList();

            return Ok(response);
        }
    }
}
