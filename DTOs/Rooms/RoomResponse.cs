
using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.RoomTypes;

namespace Hotel_Management.DTOs.Rooms
{
    public record RoomResponse(
        int Id,
        string RoomNumber,
        string Description,
        string Status,
        decimal PricePerNight,
        int MaxOccupancy,
        RoomTypeResponse RoomType,
        IEnumerable<FacilityResponse> Facilities
    );
}
