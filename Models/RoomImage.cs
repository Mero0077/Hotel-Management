namespace Hotel_Management.Models
{
    public class RoomImage : BaseModel
    {
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsCoverImage { get; set; } = false;

        public int RoomId { get; set; }
        public Room Room { get; set; } = default!;
    }
}
