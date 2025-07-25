﻿namespace Hotel_Management.DTOs.Rooms
{
    public class RoomFilterDTO
    {

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? MinCapacity { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? RoomType { get; set; }
    }

    
}
