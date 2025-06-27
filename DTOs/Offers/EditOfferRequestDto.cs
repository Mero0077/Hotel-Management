using Hotel_Management.Models;

namespace Hotel_Management.DTOs.Offers
{
    public class EditOfferRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double DiscountPercentage { get; set; }
        public ICollection<int> RoomIds { get; set; } = [];
        public bool IsActive { get; set; } = false;
    }
}
