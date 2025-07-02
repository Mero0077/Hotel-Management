using AutoMapper;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Filters;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Reservations;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationsController(ReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet]
        public bool koko()
        {
            return true;
        }

        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetReservation })]
        [HttpGet("{id}")]
        public async Task<ResponseVM<ReservationVM>> GetReservation(int id)
        {
            var result = await _reservationService.GetReservation(id);

            if (!result.IsSuccess)
                return new FailureResponseVM<ReservationVM>(result.errorCode, result.Message);

            var mapped = _mapper.Map<ReservationVM>(result.Data);
            return new SuccessResponseVM<ReservationVM>(mapped, result.Message);
        }

        [HttpGet("GetAll")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GetAllReservations })]
        public async Task<ResponseVM<IEnumerable<ReservationVM>>> GetAllReservations()
        {
            var result = await _reservationService.GetAllReservations();

            if (!result.IsSuccess)
                return new FailureResponseVM<IEnumerable<ReservationVM>>(result.errorCode, result.Message);

            var mapped = _mapper.Map<IEnumerable<ReservationVM>>(result.Data);
            return new SuccessResponseVM<IEnumerable<ReservationVM>>(mapped, result.Message);
        }

        [HttpPost("")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.AddReservation })]
        public async Task<ResponseVM<ReservationVM>> Reserve([FromBody] ReservationVM reservation)
        {
            // Let the service validate dates now!
            var reservationRequest = _mapper.Map<ReservationRequest>(reservation);
            var result = await _reservationService.ReserveRoom(reservationRequest);

            if (!result.IsSuccess)
                return new FailureResponseVM<ReservationVM>(result.errorCode, result.Message);

            var reservationVM = _mapper.Map<ReservationVM>(result.Data);
            return new SuccessResponseVM<ReservationVM>(reservationVM, result.Message);
        }

        [HttpPatch("cancel/{Id}")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.CancelReservation })]
        public async Task<ResponseVM<ReservationCancelVM>> Cancel([FromRoute] int Id)
        {
            var result = await _reservationService.Cancel(Id);

            if (!result.IsSuccess)
                return new FailureResponseVM<ReservationCancelVM>(result.errorCode, result.Message);

            var vm = _mapper.Map<ReservationCancelVM>(result.Data);
            return new SuccessResponseVM<ReservationCancelVM>(vm, result.Message);
        }

        [HttpPatch]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.EditReservation })]
        public async Task<ResponseVM<ReservationUpdateVM>> Update([FromBody] ReservationUpdateVM reservationUpdateVM)
        {
            var updateRequest = _mapper.Map<ReservationUpdateRequest>(reservationUpdateVM);
            var result = await _reservationService.Update(updateRequest);

            if (!result.IsSuccess)
                return new FailureResponseVM<ReservationUpdateVM>(result.errorCode, result.Message);

            var vm = _mapper.Map<ReservationUpdateVM>(result.Data);
            return new SuccessResponseVM<ReservationUpdateVM>(vm, result.Message);
        }
    }

}
