namespace Hotel_Management.Models
{
    public class RoomType : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Room> Rooms { get; set; } = [];
    }
}
