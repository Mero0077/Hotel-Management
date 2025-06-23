using System;

namespace Hotel_Management.Models
{
    public class Room:BaseModel
    {
        public string RoomNumber { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public RoomStatus Status { get; set; }

        public decimal PricePerNight { get; set; }

        public int MaxOccupancy { get; set; }


        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = default!;

        public ICollection<Facility> Facilities { get; set; } = [];
        public ICollection<RoomImage> RoomImages { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
        public ICollection<Offer> ApplicableOffers { get; set; } = [];
    }
}
