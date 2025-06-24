using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Models.ViewModels.Offers;

namespace Hotel_Management.DTOs.Offers
{
    public class OfferProfile : Profile
    {
        public OfferProfile() 
        {
            CreateMap<CreateOfferRequestDto, Offer>();
            CreateMap<CreateOfferVM,CreateOfferRequestDto>();

            CreateMap<EditOfferVM,EditOfferRequestDto>();
            CreateMap<EditOfferRequestDto, Offer>()
                .ForMember(des => des.Id, opt => opt.Ignore());
                

        }
    }
}
