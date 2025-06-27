using Hotel_Management.Models.Enums;

namespace Hotel_Management.Models
{
    public class RoleFeature:BaseModel
    {
        public Role Role { get; set; }
        public Features Feature { get; set; }
    }
}
