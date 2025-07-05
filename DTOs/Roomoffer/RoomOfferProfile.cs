using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Models.ViewModels.Offers;

namespace Hotel_Management.DTOs.Roomoffer
{
    public class RoomOfferProfile : Profile
    {
        public RoomOfferProfile() 
        {
            CreateMap<RoomOffer, GetOffersVM>()
                .ForMember(des => des.OfferId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(des => des.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(des => des.RoomImage, opt => opt.MapFrom(src => src.Room.RoomImages.Where(e=>e.IsCoverImage).Select(e=>e.ImageUrl).FirstOrDefault()))
                .ForMember(des => des.DiscountPercentage, opt => opt.MapFrom(src => src.Offer.DiscountPercentage))
                .ForMember(des => des.RoomId, opt => opt.MapFrom(src => src.RoomId)); 
        }
    }
}
