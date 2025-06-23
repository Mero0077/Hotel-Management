using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models.ViewModels.Reservations
{
    public class ReservationVM
    {
        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public DateTime BookingDate { get; set; }

        public int NumberOfGuests { get; set; }

        public decimal TotalCost { get; set; }

        public ReservationStatus Status { get; set; }


        public int CustomerId { get; set; }
        public int RoomId { get; set; }
    }
}
