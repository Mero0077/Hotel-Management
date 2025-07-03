using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Reviews
{
    public class ReviewVM
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Reservation ID is required.")]
        public int ReservationId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5!")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Review is required.")]
        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters.")]
        public string Comment { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}
