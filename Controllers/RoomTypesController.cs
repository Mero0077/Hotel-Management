using AutoMapper.Features;
using Azure;
using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.RoomTypes;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController(RoomTypeService roomTypeService) : ControllerBase
    {
        private readonly RoomTypeService _roomTypeService = roomTypeService;

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetAllRoomTypes })]
        [HttpGet("")]
        public async Task<ResponseVM<IEnumerable<RoomTypeResponse>>> GetAllRoomTypes(CancellationToken cancellationToken)
        {
            var response = await _roomTypeService.GetAllAsync(cancellationToken);
            return new SuccessResponseVM<IEnumerable<RoomTypeResponse>>(response);
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetRoomType })]
        [HttpGet("{id}")]
        public async Task<ResponseVM<RoomTypeResponse>> GetRoomTypeById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _roomTypeService.GetByIdAsync(id, cancellationToken);

            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddRoomType })]
        [HttpPost("")]
        public async Task<ResponseVM<RoomTypeResponse>> AddRoomType([FromBody] RoomTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomTypeService.AddAsync(request, cancellationToken);

            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.EditRoomType })]
        [HttpPut("{id}")]
        public async Task<ResponseVM<RoomTypeResponse>> UpdateRoomType([FromRoute] int id, [FromBody] RoomTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _roomTypeService.UpdateAsync(id, request, cancellationToken);

            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.DeleteRoomType })]
        [HttpDelete("{id}")]
        public async Task<ResponseVM<RoomTypeResponse>> DeleteRoomType([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _roomTypeService.DeleteAsync(id, cancellationToken);

            return result;
        }
    }
}
