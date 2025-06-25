using AutoMapper;
using Hotel_Management.Models;

namespace Hotel_Management.DTOs.RoomTypes
{
    public class RoomTypeProfile : Profile
    {
        public RoomTypeProfile()
        {
            CreateMap<RoomTypeRequest, RoomType>();
            CreateMap<RoomType, RoomTypeResponse>();
        }
    }
}
