<<<<<<< HEAD
﻿using Azure.Core;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Reservations;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private RoomService _roomService;

        public RoomsController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("")]
        public async Task<ResponseVM<IEnumerable<RoomResponse>>> GetAllRooms(CancellationToken cancellationToken)
        {
            var response = await _roomService.GetAllAsync(cancellationToken);

            return new SuccessResponseVM<IEnumerable<RoomResponse>>(response);
        }

        [HttpGet("{id}")]
        public async Task<ResponseVM<RoomResponse>> GetRoomById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await _roomService.GetByIdAsync(id, cancellationToken);

            return response;
        }

        [HttpPost("")]
        public async Task<ResponseVM<RoomResponse>> AddRoom([FromBody] AddRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.AddAsync(request, cancellationToken);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ResponseVM<RoomResponse>> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.UpdateAsync(id, request, cancellationToken);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ResponseVM<RoomResponse>> DeleteRoom([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _roomService.DeleteAsync(id, cancellationToken);
            return result;
        }
    }
}
=======
﻿//using Microsoft.AspNetCore.Http;
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
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931
