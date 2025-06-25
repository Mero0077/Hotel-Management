using AutoMapper;
using Hotel_Management.DTOs.Rooms;
using Hotel_Management.Models;

namespace Hotel_Management.DTOs.RoomImages
{
    public class RoomImageProfile : Profile
    {
        public RoomImageProfile()
        {
            //CreateMap<RoomImageRequest, RoomImage>();

            CreateMap<RoomImage, RoomImageResponse>();
        }
    }
}
