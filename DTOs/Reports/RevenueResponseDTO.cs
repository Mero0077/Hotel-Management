namespace Hotel_Management.DTOs.Reports
{
    public class RevenueResponseDTO
    {
        public DateTime Date { get; set; }
        public int BookingCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
