using Hotel_Management.DTOs.Reservation;

namespace HotelReservationSystem.api.Contracts.Rooms
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
