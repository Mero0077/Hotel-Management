using AutoMapper;
using Hotel_Management.Models;

namespace Hotel_Management.DTOs.Facilities
{
    public class FacilityProfile : Profile
    {
        public FacilityProfile()
        {
            CreateMap<FacilityRequest, Facility>();

            CreateMap<Facility, FacilityResponse>();
        }
    }
}
