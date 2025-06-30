using AutoMapper;
using Hotel_Management.DTOs.Error;
using Hotel_Management.DTOs.User;
using Hotel_Management.Models;
using Hotel_Management.Models.ViewModels.Account;

namespace Hotel_Management.DTOs.Account
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        {
            CreateMap<RegisterRequestDto,Models.User>();
            CreateMap<Models.User,UserRegisterResponseDTO>();

            CreateMap<RegisterVM,RegisterRequestDto>();

            CreateMap<UserRegisterResponseDTO,RegisterVM>();

            CreateMap<ResetPasswordVM,ResetPasswordRequestDto>();

            CreateMap<Models.User, ResetPasswordResponseVM>();

            CreateMap<LoginVM, LoginDTO>();
        }
    }
}
