using AutoMapper;
using Hotel_Management.DTOs.Error;
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
        public ReservationService(IMapper mapper, GeneralRepository<Reservation> reservationRepository, GeneralRepository<Room> RoomRepository)
        {
            _ReservationRepository = reservationRepository;
            _RoomRepository = RoomRepository;
            this.mapper = mapper;
        }

        private static bool DateChecker(DateTime StartDate, DateTime EndDate)
        {
            return StartDate<DateTime.UtcNow.Date || EndDate <= StartDate.Date;
        }

        private static decimal CalculateTotalCost(DateTime StartDate, DateTime EndDate,decimal price)
        {
            return (EndDate - StartDate).Days * price;
        }

<<<<<<< HEAD
            var room = await _RoomRepository.GetOneWithTrackingAsync(r => r.Id == req.RoomId);
=======
        private async Task ChangeRoomStatus(Room room,RoomStatus roomStatus)
        {
            room.Status = roomStatus;
            await _RoomRepository.UpdateIncludeAsync(room, nameof(room.Status));
        }
        public List<Reservation> GetAllReservations()
        {
            return _ReservationRepository.GetAll().ToList();
        }

        public async Task<Reservation> GetReservation(int Id)
        {
           return await _ReservationRepository.GetOneWithTracking(e=>e.Id == Id);
        }
        public async Task<ResponseDTO<ReservationRequest>> ReserveRoom(ReservationRequest req)
        {
           
            var room = await _RoomRepository.GetOneWithTracking(r => r.Id == req.RoomId);
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931

            if (room == null)
                return new ErrorFailDTO<ReservationRequest>(ErrorCode.RoomNotFound, "Room not found");

            if (room.Status == RoomStatus.Occupied)
                return new ErrorFailDTO<ReservationRequest>(ErrorCode.ReservationExists, "Room is already booked");

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, room.PricePerNight);
            var reservation = mapper.Map<Reservation>(req);
            await _ReservationRepository.AddAsync(reservation);

           await ChangeRoomStatus(room, RoomStatus.Occupied);

            return new ErrorSuccessDTO<ReservationRequest>(req, "Reservation successful");
        }

        public async Task<Reservation> Update(ReservationUpdateRequest req)
        {

            var dto = await _ReservationRepository.Get(r => r.Id == req.Id).Select(r => new
            {
            Reservation = r,              
            RoomPrice = r.Room.PricePerNight}).FirstOrDefaultAsync();


            if (dto == null)
                return null;

            req.TotalCost = CalculateTotalCost(req.CheckInDate, req.CheckOutDate, dto.RoomPrice);
<<<<<<< HEAD
            var UpdatedRes= mapper.Map<Reservation>(req);
            await _ReservationRepository.UpdateAsync(UpdatedRes);
            return UpdatedRes;
=======
            var updatedReservation = mapper.Map<Reservation>(req);
            updatedReservation.Id=req.Id;

            await _ReservationRepository.UpdateIncludeAsync(updatedReservation, nameof(updatedReservation.CheckInDate),nameof(updatedReservation.CheckOutDate),
                                                          nameof(updatedReservation.BookingDate),nameof(updatedReservation.NumberOfGuests),
                                                          nameof(updatedReservation.TotalCost),nameof(updatedReservation.Status),nameof(updatedReservation.RoomId));

            return updatedReservation;
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931

        }
        public async Task<Reservation> Cancel(int id)
        {
<<<<<<< HEAD
            var reservation= await _ReservationRepository.GetOneWithTrackingAsync(e=>e.Id == id);
            if (reservation == null)
=======
            var reserv = await _ReservationRepository.Get(e => e.Id == id).Select(e => new
            {
                Reservation=e,
                Room=e.Room
            }).FirstOrDefaultAsync();
            if (reserv == null)
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931
                return null;

            if (DateChecker(reserv.Reservation.CheckInDate, reserv.Reservation.CheckOutDate)) return null;

<<<<<<< HEAD
            reservation.Status=ReservationStatus.Cancelled;
            await _ReservationRepository.UpdateAsync(reservation);
            return reservation;
=======
            reserv.Reservation.Status=ReservationStatus.Cancelled;
            await _ReservationRepository.UpdateIncludeAsync(reserv.Reservation,nameof(reserv.Reservation.Status));

            await ChangeRoomStatus(reserv.Room, RoomStatus.Available);
            return reserv.Reservation;
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931

        }   
    }
}
