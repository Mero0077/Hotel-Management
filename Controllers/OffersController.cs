﻿using AutoMapper;
using Hotel_Management.DTOs.Offers;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Offers;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly OfferService _OfferService;

        public OffersController(IMapper mapper, OfferService offerService)
        {
            this._mapper = mapper;
            this._OfferService = offerService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseVM<IEnumerable<GetOffersVM>>> GetAvaliableOffers()
        {
            var response = await _OfferService.GetAvaliableOffersAsync();
            return new SuccessResponseVM<IEnumerable<GetOffersVM>>(response);
        }


        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddOffer })]
        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferVM createOfferVM)
        {
            /* This is used to check if the user is the same user from the token
             until now there is no tokens so it will be commented
            */

            //var hotelStaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (hotelStaffId == null) return new FailureResponseVM<CreateOfferVM>(ErrorCode.UserUnauthorized);

            var offer = _mapper.Map<CreateOfferRequestDto>(createOfferVM);

            var res = await _OfferService.CreateOfferAsync(offer);

            return StatusCode((int)res.errorCode, res);
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.EditOffer })]
        [HttpPut("Offers/{offerId}")]
        public async Task<IActionResult> EditOffer([FromRoute] int offerId, [FromBody] EditOfferVM editOfferVM)
        {
            /* This is used to check if the user is the same user from the token
            until now there is no tokens so it will be commented
           */

            //var hotelStaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (hotelStaffId == null) return new FailureResponseVM<CreateOfferVM>(ErrorCode.UserUnauthorized);

            var offer = _mapper.Map<EditOfferRequestDto>(editOfferVM);

            var res = await _OfferService.EditOfferAsync(offerId,offer);

            return StatusCode((int)res.errorCode, res);
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.DeleteOffer })]
        [HttpDelete("Offers/{offerId}")]
        public async Task<IActionResult> DeleteOffer([FromRoute] int offerId)
        {
            /* This is used to check if the user is the same user from the token
           until now there is no tokens so it will be commented
          */

            //var hotelStaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (hotelStaffId == null) return new FailureResponseVM<CreateOfferVM>(ErrorCode.UserUnauthorized);

            var res = await _OfferService.DeleteOfferAsync(offerId);
            return StatusCode((int)res.errorCode, res);
        }
    }

}
