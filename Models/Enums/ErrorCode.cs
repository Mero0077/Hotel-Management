namespace Hotel_Management.Models.Enums
{
    public enum ErrorCode
    {
        NoError = 0,

        #region Reservation
        ReservationNotFound = 100,
        InvalidReservation = 101,
        ReservationExists = 102,
        #endregion


        #region Room
        RoomNotFound = 200,
        InvalidRoom = 201,
        RoomBooked = 202,
        RoomAlreadyExists = 203,
        #endregion

        #region RoomType
        RoomTypeNotFound = 300,
        RoomTypeAlreadyExists = 301,
        RoomTypeInUse = 302,
        #endregion

        #region Facility
        FacilityNotFound = 400,
        FacilityAlreadyExists = 401,
        FacilityInUse = 402,
        #endregion

        #region RoomImage
        RoomImageNotFound = 500,
        RoomImageExtensionIsNotValid = 501,
        #endregion

    }
}
