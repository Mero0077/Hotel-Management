using AutoMapper;
using Hotel_Management.DTOs.RoomTypes;
using Hotel_Management.Models;

namespace Hotel_Management.DTOs.Rooms
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomRequest, Room>();

            CreateMap<Room, RoomResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Facilities, opt => opt.MapFrom(src => src.Facilities));
        }
    }
}
