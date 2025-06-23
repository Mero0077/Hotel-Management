using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _facilityService = facilityService;

        [HttpGet("")]
        public async Task<IActionResult> GetAllFacilities(CancellationToken cancellationToken)
        {
            var facilities = await _facilityService.GetAllAsync(cancellationToken);
            return Ok(facilities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacilityById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _facilityService.GetByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddFacility([FromBody] FacilityRequest request, CancellationToken cancellationToken)
        {
            var result = await _facilityService.AddAsync(request, cancellationToken);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetFacilityById), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility([FromRoute] int id, [FromBody] FacilityRequest request, CancellationToken cancellationToken)
        {
            var result = await _facilityService.UpdateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _facilityService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
