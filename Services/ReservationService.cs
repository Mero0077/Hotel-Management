using AutoMapper;
using Hotel_Management.DTOs.Error;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Exceptions;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class ReservationService
    {
        private readonly IMapper mapper;
        private readonly GeneralRepository<Reservation> _ReservationRepository;
        private readonly GeneralRepository<Room> _RoomRepository;

        public ReservationService(IMapper mapper, GeneralRepository<Reservation> reservationRepository, GeneralRepository<Room> roomRepository)
        {
            _ReservationRepository = reservationRepository;
            _RoomRepository = roomRepository;
            this.mapper = mapper;
        }

        private static bool DateChecker(DateTime startDate, DateTime endDate)
        {
            return startDate < DateTime.UtcNow.Date || endDate <= startDate.Date;
        }

        private static decimal CalculateTotalCost(DateTime startDate, DateTime endDate, decimal price)
        {
            return (endDate - startDate).Days * price;
        }

        private async Task ChangeRoomStatus(Room room, RoomStatus roomStatus)
        {
            room.Status = roomStatus;
            await _RoomRepository.UpdateIncludeAsync(room, nameof(room.Status));
        }

        public async Task<ResponseDTO<List<Reservation>>> GetAllReservations()
        {
            var reservations = _ReservationRepository.GetAll().ToList();
            return new ErrorSuccessDTO<List<Reservation>>(reservations, "All reservations retrieved.");
        }

        public async Task<ResponseDTO<Reservation>> GetReservation(int id)
        {
            var reservation = await _ReservationRepository.GetOneWithTrackingAsync(e => e.Id == id);

            if (reservation == null)
                return new ErrorFailDTO<Reservation>(ErrorCode.ReservationNotFound, "Reservation not found");

            return new ErrorSuccessDTO<Reservation>(reservation, "Reservation retrieved.");
        }

        public async Task<ResponseDTO<ReservationRequest>> ReserveRoom(ReservationRequest req)
        {
            var room = await _RoomRepository.GetOneWithTrackingAsync(r => r.Id == req.RoomId);

            if (room == null)
                return new ErrorFailDTO<ReservationRequest>(ErrorCode.RoomNotFound, "Room not found");

            if (room.Status == RoomStatus.Occupied)
                return new ErrorFailDTO<ReservationRequest>(ErrorCode.ReservationExists, "Room is already booked");

            if (DateChecker(req.CheckInDate, req.CheckOutDate))
                return new ErrorFailDTO<ReservationRequest>(ErrorCode.ReservateDateInvalid, "Invalid reservation dates.");

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, room.PricePerNight);
            var reservation = mapper.Map<Reservation>(req);

           
            await _ReservationRepository.AddAsync(reservation);
            await _ReservationRepository.SaveChangesAsync();
            

            await ChangeRoomStatus(room, RoomStatus.Occupied);

            return new ErrorSuccessDTO<ReservationRequest>(req, "Reservation successful.");
        }

        public async Task<ResponseDTO<Reservation>> Update(ReservationUpdateRequest req)
        {
            var dto = await _ReservationRepository.Get(r => r.Id == req.Id)
                .Select(r => new
                {
                    Reservation = r,
                    RoomPrice = r.Room.PricePerNight
                }).FirstOrDefaultAsync();

            if (dto == null)
                return new ErrorFailDTO<Reservation>(ErrorCode.ReservationNotFound, "Reservation not found");

            if (DateChecker(req.CheckInDate, req.CheckOutDate))
                return new ErrorFailDTO<Reservation>(ErrorCode.ReservateDateInvalid, "Invalid dates for update");

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, dto.RoomPrice);
            var updatedReservation = mapper.Map<Reservation>(req);
            updatedReservation.Id = req.Id;

            await _ReservationRepository.UpdateIncludeAsync(
                updatedReservation,
                nameof(updatedReservation.CheckInDate),
                nameof(updatedReservation.CheckOutDate),
                nameof(updatedReservation.BookingDate),
                nameof(updatedReservation.NumberOfGuests),
                nameof(updatedReservation.TotalCost),
                nameof(updatedReservation.Status),
                nameof(updatedReservation.RoomId)
            );
            await _ReservationRepository.SaveChangesAsync();

            return new ErrorSuccessDTO<Reservation>(updatedReservation, "Reservation updated successfully.");
        }

            public async Task<ResponseDTO<Reservation>> Cancel(int id)
            {
                var reserv = await _ReservationRepository.Get(e => e.Id == id)
                    .Select(e => new
                    {
                        Reservation = e,
                        Room = e.Room
                    }).FirstOrDefaultAsync();

                if (reserv == null)
                    return new ErrorFailDTO<Reservation>(ErrorCode.ReservationNotFound, "Reservation not found");

                if (DateChecker(reserv.Reservation.CheckInDate, reserv.Reservation.CheckOutDate))
                    return new ErrorFailDTO<Reservation>(ErrorCode.StartDateMustBeBeforeEndDate, "Invalid dates for cancellation.");

                reserv.Reservation.Status = ReservationStatus.Cancelled;
                await _ReservationRepository.UpdateIncludeAsync(reserv.Reservation, nameof(reserv.Reservation.Status));
                  await _ReservationRepository.SaveChangesAsync();

                await ChangeRoomStatus(reserv.Room, RoomStatus.Available);
                await _ReservationRepository.SaveChangesAsync();

                return new ErrorSuccessDTO<Reservation>(reserv.Reservation, "Reservation cancelled successfully.");
            }
    }

}
