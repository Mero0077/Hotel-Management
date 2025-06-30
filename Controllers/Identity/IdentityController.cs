using AutoMapper;
using Hotel_Management.DTOs.Account;
using Hotel_Management.DTOs.Error;
using Hotel_Management.DTOs.User;
using Hotel_Management.Helper;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Account;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Services;
using Hotel_Management.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers.Identity
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private UserService _userService;
        private readonly IMapper _mapper;

        public IdentityController(UserService userService,IMapper mapper)
        {
            _userService = userService;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ResponseVM<string>> Login(LoginVM loginVM)
        {
            var mapped = _mapper.Map<LoginDTO>(loginVM);

            var user = await _userService.LoginAsync(mapped);
            if (user.IsSuccess)
                return new SuccessResponseVM<string>(user.Data);

            return new FailureResponseVM<string>(user.errorCode);
        }

        [HttpPost]
        public async Task<ResponseVM<RegisterVM>> Register([FromBody] RegisterVM registerVM)
        {
            var mapped = _mapper.Map<RegisterRequestDto>(registerVM);

            var userRegister = await _userService.RegisterAsync(mapped);
            if (userRegister == null)
                return new FailureResponseVM<RegisterVM>(ErrorCode.PasswordIsnotMatched);

            var responseMapped = _mapper.Map<RegisterVM>(userRegister);
            return new SuccessResponseVM<RegisterVM>(responseMapped);
        }


        [HttpPost]
        public async Task<ResponseVM<bool>> SendOTP(SendEmailVM sendEmailVM)
        {
            var res = await _userService.SendOtpToMailAsync(sendEmailVM.Email);
            if (res == false)
                return new FailureResponseVM<bool>(ErrorCode.EmailNotFound);
            return new SuccessResponseVM<bool>(res);
        }
        [HttpPut]
        public async Task<ResponseVM<ResetPasswordResponseVM>> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var mapped = _mapper.Map<ResetPasswordRequestDto>(resetPasswordVM);
            var res = await  _userService.ResetPasswordAsync(mapped);


            if(res is ErrorSuccessDTO<User> success)
            {
                var mappedResponse = _mapper.Map<ResetPasswordResponseVM>(success.Data);
                return new SuccessResponseVM<ResetPasswordResponseVM>(mappedResponse);
            }
            return new FailureResponseVM<ResetPasswordResponseVM>(res.errorCode);
        }
    }
}
