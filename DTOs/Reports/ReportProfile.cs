using AutoMapper;
using Hotel_Management.Models.ViewModels.Reports;

namespace Hotel_Management.DTOs.Reports
{
    public class ReportProfile : Profile
    {
        ReportProfile() 
        {
            CreateMap<Models.Reservation, GetBookingReportResponseDTO>()
                .ForMember(des => des.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(des => des.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email));
            CreateMap<GetBookingReportVM, GetBokkingReportRequestDTO>();
            CreateMap<GetBookingReportResponseDTO,BookingReportVM>();
        }
    }
}
