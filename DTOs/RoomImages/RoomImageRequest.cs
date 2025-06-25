namespace Hotel_Management.DTOs.RoomImages
{
    public class RoomImageRequest
    {
        public int Id { get; set; }
        public required IFormFile RoomImage { get; set; }
    }
}
