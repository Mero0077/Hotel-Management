using AutoMapper.Features;
using Azure.Core;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Reservations;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetAllRooms })]
        [HttpGet("")]
        public async Task<ResponseVM<IEnumerable<RoomResponse>>> GetAllRooms(CancellationToken cancellationToken)
        {
            var response = await _roomService.GetAllAsync(cancellationToken);

            return new SuccessResponseVM<IEnumerable<RoomResponse>>(response);
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetRoom })]
        [HttpGet("{id}")]
        public async Task<ResponseVM<RoomResponse>> GetRoomById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await _roomService.GetByIdAsync(id, cancellationToken);

            return response;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddRoom })]
        [HttpPost("")]
        public async Task<ResponseVM<RoomResponse>> AddRoom([FromBody] AddRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.AddAsync(request, cancellationToken);
            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.EditRoom })]
        [HttpPut("{id}")]
        public async Task<ResponseVM<RoomResponse>> UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomService.UpdateAsync(id, request, cancellationToken);
            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.DeleteRoom })]
        [HttpDelete("{id}")]
        public async Task<ResponseVM<RoomResponse>> DeleteRoom([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _roomService.DeleteAsync(id, cancellationToken);
            return result;
        }
    }
}

