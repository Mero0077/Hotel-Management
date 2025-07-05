namespace Hotel_Management.Models.ViewModels.Reports
{
    public class RevenueReportVM
    {
        public DateTime Date { get; set; }
        public int BookingCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
