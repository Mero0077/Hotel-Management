﻿namespace Hotel_Management.Models
{
    public class CustomerFeedback:BaseModel
    {
        public int UserId { get; set; }
        public User user { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int ReservationId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }

        public bool IsApproved { get; set; } = false;

    }
}
