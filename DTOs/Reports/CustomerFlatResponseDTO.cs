namespace Hotel_Management.DTOs.Reports
{
    public class CustomerFlatResponseDTO
    {
        public Guid CustomerId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int TotalBookings { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
