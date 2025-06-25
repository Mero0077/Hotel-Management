using Hotel_Management.DTOs.RoomImages;
using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Rooms
{
    public record UpdateRoomRequest
    (
        string RoomNumber,
        string Description,
        RoomStatus Status,
        decimal PricePerNight,
        int MaxOccupancy,
        int RoomTypeId,
        List<int> FacilityIds,
        IEnumerable<RoomImageRequest> RoomImages
    );
}
