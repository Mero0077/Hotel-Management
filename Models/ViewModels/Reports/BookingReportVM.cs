namespace Hotel_Management.Models.ViewModels.Reports
{
    public class BookingReportVM
    {
        public string CustomerEmail { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

    }
}
