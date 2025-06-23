using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Reservation
{
    public class CancelReservationRequest
    {
        public int Id { get; set; }
        public ReservationStatus reservationStatus { get; set; }
    }
}
