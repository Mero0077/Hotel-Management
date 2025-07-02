using AutoMapper.Features;
using Azure;
using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using HotelReservationSystem.api.Services.FacilitiesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController(FacilityService facilityService) : ControllerBase
    {
        private readonly FacilityService _facilityService = facilityService;


        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] {Features.GetAllFacilities })]
        [HttpGet("")]
        public async Task<ResponseVM<IEnumerable<FacilityResponse>>> GetAllFacilities(CancellationToken cancellationToken)
        {
            var response = await _facilityService.GetAllAsync(cancellationToken);
            return new SuccessResponseVM<IEnumerable<FacilityResponse>>(response);
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetFacility })]
        [HttpGet("{id}")]
        public async Task<ResponseVM<FacilityResponse>> GetFacilityById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _facilityService.GetByIdAsync(id, cancellationToken);
            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddFacility })]
        [HttpPost("")]
        public async Task<ResponseVM<FacilityResponse>> AddFacility([FromBody] FacilityRequest request, CancellationToken cancellationToken)
        {
            var result = await _facilityService.AddAsync(request, cancellationToken);
            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.EditFacility })]
        [HttpPut("{id}")]
        public async Task<ResponseVM<FacilityResponse>> UpdateFacility([FromRoute] int id, [FromBody] FacilityRequest request, CancellationToken cancellationToken)
        {
            var result = await _facilityService.UpdateAsync(id, request, cancellationToken);
            return result;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.DeleteFacility })]
        [HttpDelete("{id}")]
        public async Task<ResponseVM<FacilityResponse>> DeleteFacility([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _facilityService.DeleteAsync(id, cancellationToken);
            return result;
        }
    }
}

﻿
