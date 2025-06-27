using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.User
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
    }
}
