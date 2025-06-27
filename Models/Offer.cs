namespace Hotel_Management.Models
{
    public class Offer : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountPercentage { get; set; }
        public ICollection<RoomOffer> RoomOffers { get; set; } = [];
    }
}
