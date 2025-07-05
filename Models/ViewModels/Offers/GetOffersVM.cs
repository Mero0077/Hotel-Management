namespace Hotel_Management.Models.ViewModels.Offers
{
    public class GetOffersVM
    {
        public int OfferId { get; set; }
        public int RoomId { get; set; }
        public double DiscountPercentage { get; set; }
        public string RoomImage { get; set; }
        public string RoomNumber { get; set; }
    }
}
