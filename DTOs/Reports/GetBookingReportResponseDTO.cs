namespace Hotel_Management.DTOs.Reports
{
    public class GetBookingReportResponseDTO
    {
        public string CustomerEmail { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
