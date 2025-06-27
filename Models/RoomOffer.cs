namespace Hotel_Management.Models
{
    public class RoomOffer : BaseModel
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int OfferId { get; set; }

        public Offer Offer { get; set; }
    }
}
