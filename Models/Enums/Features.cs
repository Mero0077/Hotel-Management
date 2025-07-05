namespace Hotel_Management.Models.Enums
{
    public enum Features
    {
   
        None = 0,

        // Reservation
        GetReservation = 1,
        GetAllReservations = 2,
        AddReservation = 3,
        CancelReservation = 4,
        EditReservation = 5,
        DeleteReservation = 6,

        // Room
        GetRoom = 10,
        GetAllRooms = 11,
        AddRoom = 12,
        EditRoom = 13,
        DeleteRoom = 14,
        ChangeRoomStatus = 15,

        // RoomType
        GetRoomType = 20,
        GetAllRoomTypes = 21,
        AddRoomType = 22,
        EditRoomType = 23,
        DeleteRoomType = 24,

        // Facility
        GetFacility = 30,
        GetAllFacilities = 31,
        AddFacility = 32,
        EditFacility = 33,
        DeleteFacility = 34,

        // Offer
        GetOffer = 40,
        GetAllOffers = 41,
        AddOffer = 42,
        EditOffer = 43,
        DeleteOffer = 44,
        ApplyOffer = 45,

        //Review
        AddReview=50,
        ApproveReview=51,



        //Report
        BookingReport=60,
        GenerateBookingReportPDF=61,



    }

}

