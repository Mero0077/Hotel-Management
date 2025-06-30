using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models
{
    public class User:BaseModel
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
