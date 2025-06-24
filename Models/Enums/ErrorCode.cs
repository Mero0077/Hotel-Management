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
        #endregion


        #region User
        UserUnauthorized = 300,
        #endregion

        #region Offer
        StartDateAlreadyExceed = 400,
        StartDateMustBeBeforeEndDate=401,
        OfferNotFound=402,
        ThereShouldBeAtLeastOneRoom=403
        #endregion


    }
}
