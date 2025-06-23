using AutoMapper;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class ReservationService
    {
        private IMapper mapper;
        private GeneralRepository<Reservation> _ReservationRepository;
        private GeneralRepository<Room> _RoomRepository;
        public ReservationService(IMapper mapper)
        {
            _ReservationRepository = new GeneralRepository<Reservation>();
            _RoomRepository = new GeneralRepository<Room>();
            this.mapper = mapper;
        }

        public List<Reservation> GetAllReservations()
        {
            return _ReservationRepository.GetAll().ToList();
        }

        private static bool DateChecker(DateTime StartDate, DateTime EndDate)
        {
            return StartDate<DateTime.UtcNow.Date || EndDate <= StartDate.Date;
        }

        private static decimal CalculateTotalCost(DateTime StartDate, DateTime EndDate,decimal price)
        {
            return (EndDate - StartDate).Days * price;
        }
        public async Task<ResponseVM<ReservationRequest>> ReserveRoom(ReservationRequest req)
        {
            if (DateChecker(req.CheckInDate,req.CheckInDate))
                return new FailureResponseVM<ReservationRequest>(ErrorCode.InvalidReservation, "Invalid date");

            var room = await _RoomRepository.GetOneWithTracking(r => r.Id == req.RoomId);

            if (room == null)
                return new FailureResponseVM<ReservationRequest>(ErrorCode.RoomNotFound, "Room not found");

            if (room.Status == RoomStatus.Occupied)
                return new FailureResponseVM<ReservationRequest>(ErrorCode.ReservationExists, "Room is already booked");

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, room.PricePerNight);
            var reservation = mapper.Map<Reservation>(req);
            await _ReservationRepository.Add(reservation);

            return new SuccessResponseVM<ReservationRequest>(req, "Reservation successful");
        }

        public async Task<Reservation> Update(ReservationUpdateRequest req)
        {
            if (DateChecker(req.CheckInDate,req.CheckOutDate)) return null;

            var dto = await _ReservationRepository.Get(r => r.Id == req.Id).Select(r => new
        {
            Reservation = r,              
            RoomPrice = r.Room.PricePerNight
        }).FirstOrDefaultAsync();


            if (dto == null)
                return null;

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, dto.RoomPrice);
            var UpdatedRes= mapper.Map<Reservation>(req);
            await _ReservationRepository.Update(UpdatedRes);
            return UpdatedRes;

        }
        public async Task<Reservation> Cancel(int id)
        {
            var reservation= await _ReservationRepository.GetOneWithTracking(e=>e.Id == id);
            if (reservation == null)
                return null;

            if (DateChecker(reservation.CheckInDate, reservation.CheckOutDate)) return null;

            reservation.Status=ReservationStatus.Cancelled;
            await _ReservationRepository.Update(reservation);
            return reservation;

        }   
    }
}
