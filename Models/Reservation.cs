using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models
{
    public class Reservation:BaseModel
    {
        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public DateTime BookingDate { get; set; }

        public int NumberOfGuests { get; set; }

        public decimal TotalCost { get; set; }

        public ReservationStatus Status { get; set; }


        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = default!;

        public CustomerFeedback Feedback { get; set; } = default!;
    }
}
