using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models.ViewModels.Reservations
{
    public class ReservationCancelVM
    {
        public int Id { get; set; }
        public ReservationStatus reservationStatus { get; set; }

    }
}
