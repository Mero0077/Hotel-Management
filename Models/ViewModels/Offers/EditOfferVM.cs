namespace Hotel_Management.Models.ViewModels.Offers
{
    public class EditOfferVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountPercentage { get; set; }
        public ICollection<int> RoomIds { get; set; } = [];
        public bool IsActive { get; set; } = false;
    }
}
