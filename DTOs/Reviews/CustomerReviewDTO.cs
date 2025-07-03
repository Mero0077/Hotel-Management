using Hotel_Management.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.DTOs.Reviews
{
    public class CustomerReviewDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5!")]
        public int ReservationId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}
