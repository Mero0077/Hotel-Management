using AutoMapper;
using Hotel_Management.DTOs.Reservation;
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
        private ReservationService _reservationService;
        private IMapper _mapper;

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


        private static bool DateChecker(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < DateTime.UtcNow.Date || EndDate <= StartDate.Date;
        }

        [HttpGet("{id}")]
        public ResponseVM<ReservationVM> GetReservation(int id)
        {
            var res= _reservationService.GetReservation(id);
            if (res == null)
                return new FailureResponseVM<ReservationVM>(ErrorCode.ReservationNotFound, "Reservation not found!");

            var mapped= _mapper.Map<ReservationVM>(res);
                return new SuccessResponseVM<ReservationVM>(mapped, "Reservation retrieved!.");

        }

        [HttpGet("GetAll")]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments =new object[] {Features.GetAllReservation})]
        public ResponseVM<IEnumerable<ReservationVM>> GetAllReservations()
        {
            var res= _reservationService.GetAllReservations();
            var mapped= _mapper.Map<IEnumerable<ReservationVM>>(res);
            return new SuccessResponseVM<IEnumerable<ReservationVM>>(mapped);
        }
        [HttpPost("")]
        public async Task<ResponseVM<ReservationVM>> Reserve([FromBody] ReservationVM reservation)
        {
            if (DateChecker(reservation.CheckInDate, reservation.CheckInDate))
                return new FailureResponseVM<ReservationVM>(ErrorCode.ReservateDateInvalid,"You must enter a valid date!");

            var Reservation = _mapper.Map<ReservationRequest>(reservation);
            var result = await _reservationService.ReserveRoom(Reservation);

            if (!result.IsSuccess)
                return new FailureResponseVM<ReservationVM>(result.errorCode, result.Message);

            var reservationVM = _mapper.Map<ReservationVM>(result.Data);
            return new SuccessResponseVM<ReservationVM>(reservationVM, "Reservation successful.");
        }

        [HttpPatch("cancel/{Id}")]
        public async Task<ResponseVM<ReservationCancelVM>> Cancel([FromRoute] int Id)
        {
            var res = await _reservationService.Cancel(Id);
            if(res==null)
                return new FailureResponseVM<ReservationCancelVM>(ErrorCode.ReservationNotFound, "Reservation not found");

            var vm = _mapper.Map<ReservationCancelVM>(res);
                return new SuccessResponseVM<ReservationCancelVM>(vm, "Reservation cancelled");
        }

        [HttpPatch]
        public async Task<ResponseVM<ReservationUpdateVM>> Update([FromBody] ReservationUpdateVM reservationUpdateVM)
        {
            if (DateChecker(reservationUpdateVM.CheckInDate, reservationUpdateVM.CheckInDate))
                return new FailureResponseVM<ReservationUpdateVM>(ErrorCode.ReservateDateInvalid, "You must enter a valid date!");

            var Reservation = _mapper.Map<ReservationUpdateRequest>(reservationUpdateVM);
            var res= await _reservationService.Update(Reservation);
            if (res==null)
                return new FailureResponseVM<ReservationUpdateVM>(ErrorCode.ReservationNotFound, "Reservation not found");

            var vm = _mapper.Map<ReservationUpdateVM>(res);
            return new SuccessResponseVM<ReservationUpdateVM>(vm, "Reservation Updated");
        }

    }
}
