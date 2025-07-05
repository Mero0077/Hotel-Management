namespace Hotel_Management.Models.Enums
{
    public enum ErrorCode
    {
        NoError = 0,
        InternalServerError=1,
        UnauthorizedAccess=2,

        #region Reservation
        ReservationNotFound = 100,
        InvalidReservation = 101,
        ReservationExists = 102,
        ReservateDateInvalid=103,
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



        #region User
        UserUnauthorized = 600,
        PasswordIsnotMatched=601,
        EmailNotFound =603,
        OTPIsNotCorrect=604,
        OTPExpired=605,
        EmailOrPasswordIsWrong=606,
        #endregion

      


        #region Review
        ReviewNotFound = 700,
        DuplicateReview = 701,
        AlreadyApproved = 703,
        #endregion

        #region Offer
        StartDateAlreadyExceed = 800,
        StartDateMustBeBeforeEndDate = 801,
        OfferNotFound = 802,
        ThereShouldBeAtLeastOneRoom = 803,
        #endregion



    }
}
