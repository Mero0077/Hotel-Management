namespace Hotel_Management.Models.ViewModels.Reports
{
    public class CustomerReportVM
    {
        public string Email { get; set; } = string.Empty;
        public int TotalBookings { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
