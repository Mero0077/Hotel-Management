//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Hotel_Management.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RoomsController : ControllerBase
//    {
//        private readonly IRoomService _roomService = roomService;

//        [HttpGet("")]
//        public async Task<IActionResult> GetAllRooms(CancellationToken cancellationToken)
//        {
//            var rooms = await _roomService.GetAllAsync(cancellationToken);
//            return Ok(rooms);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetRoomById([FromRoute] int id, CancellationToken cancellationToken)
//        {
//            var result = await _roomService.GetByIdAsync(id, cancellationToken);
//            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
//        }

//        [HttpPost("")]
//        public async Task<IActionResult> AddRoom([FromBody] RoomRequest request, CancellationToken cancellationToken)
//        {
//            var result = await _roomService.AddAsync(request, cancellationToken);
//            return result.IsSuccess
//                ? CreatedAtAction(nameof(GetRoomById), new { id = result.Value.Id }, result.Value)
//                : result.ToProblem();
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateRoom([FromRoute] int id, [FromBody] RoomRequest request, CancellationToken cancellationToken)
//        {
//            var result = await _roomService.UpdateAsync(id, request, cancellationToken);
//            return result.IsSuccess ? NoContent() : result.ToProblem();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteRoom([FromRoute] int id, CancellationToken cancellationToken)
//        {
//            var result = await _roomService.DeleteAsync(id, cancellationToken);
//            return result.IsSuccess ? NoContent() : result.ToProblem();
//        }
//    }
//}
