using Hotel_Management.Models.Enums;

namespace Hotel_Management.DTOs.Reservation
{
    public record RoomRequest(
        string RoomNumber,
        string Description,
        RoomStatus Status,
        decimal PricePerNight,
        int MaxOccupancy,
        int RoomTypeId,
        List<int> FacilityIds
    );
}
