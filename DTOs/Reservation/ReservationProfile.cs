using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Models.ViewModels.Reservations;

namespace Hotel_Management.DTOs.Reservation
{
    public class ReservationProfile:Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationVM, ReservationRequest>();
            CreateMap<ReservationRequest, Hotel_Management.Models.Reservation>();
            CreateMap<Hotel_Management.Models.Reservation, ReservationVM>();
        }
    }
}
