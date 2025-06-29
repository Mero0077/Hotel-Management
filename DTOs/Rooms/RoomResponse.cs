<<<<<<< HEAD
﻿using Hotel_Management.DTOs.Facilities;
using Hotel_Management.DTOs.RoomTypes;
=======
﻿using Hotel_Management.DTOs.Reservation;
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931

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
