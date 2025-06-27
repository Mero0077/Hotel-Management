using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Offers
{
    public class CreateOfferVM
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [Range(0,50)]
        public double DiscountPercentage { get; set; }

        public ICollection<int> RoomIds { get; set; } = [];

        public bool IsActive { get; set; } = false;
    }
}
