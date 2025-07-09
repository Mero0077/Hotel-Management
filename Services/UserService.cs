using AutoMapper;
using Hotel_Management.DTOs.Account;
using Hotel_Management.DTOs.Error;
using Hotel_Management.DTOs.User;
using Hotel_Management.Helper;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Repositories;
using Hotel_Management.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Hotel_Management.Services
{
    public class UserService
    {
        private readonly GeneralRepository<User> _generalRepository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;

        public UserService(GeneralRepository<User>  generalRepository,IEmailService emailService,IMemoryCache memoryCache,IMapper mapper) 
        { 
            _generalRepository = generalRepository;
            this._emailService = emailService;
            this._memoryCache = memoryCache;
            this._mapper = mapper;
        }

        public async Task<ResponseDTO<string>> LoginAsync(LoginDTO request)
        {
            // hashing for both inputs ?????
     
            var hashedUserName = HashHelper.HashUserName(request.UserName);
            var user = await _generalRepository.Get(u => u.UserName == hashedUserName).FirstOrDefaultAsync();
            if (user == null)
                return new ErrorFailDTO<string>(ErrorCode.EmailNotFound);

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user,user.Password,request.Password);

            if (result == PasswordVerificationResult.Failed)
                return new ErrorFailDTO<string>(ErrorCode.EmailOrPasswordIsWrong);

            string token = Helper.GenerateToken.Generate(user.Id,"Saad", user.Role);


            return new ErrorSuccessDTO<string>(token);
        }

        public async Task<UserRegisterResponseDTO?> RegisterAsync(RegisterRequestDto request)
        {
            if (request.ConfirmedPassword != request.Password)
                return null;

            var user = _mapper.Map<User>(request);
            user.UserName=HashHelper.HashUserName(user.UserName);

            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, request.Password);

            await _generalRepository.AddAsync(user);
            await _generalRepository.SaveChangesAsync();
            var response = _mapper.Map<UserRegisterResponseDTO>(user);

            return response;
        }

        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> SendOtpToMailAsync(string mail)
        {
            var userMail  = await _generalRepository.Get(e=>e.Email == mail).FirstOrDefaultAsync();
            if (userMail == null)
                return false;
           string otp = GenerateOTP();
           _memoryCache.Set(mail,otp,TimeSpan.FromMinutes(5));
           await _emailService.SendOTP(mail,otp);
           return true;
        }

        public async Task<ResponseDTO<User>?> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            if(_memoryCache.TryGetValue(request.Email,out string cachedOTP))
            {
                if(cachedOTP == request.OTP)
                {
                    var user = await _generalRepository.GetOneWithTrackingAsync(e=>e.Email==request.Email);
                    if (user == null)
                        return new ErrorFailDTO<User>(ErrorCode.EmailNotFound);

                    var hasher = new PasswordHasher<User>();
                    user.Password = hasher.HashPassword(user, request.Password);
                    await _generalRepository.UpdateIncludeAsync(user,nameof(User.Password));
                    await _generalRepository.SaveChangesAsync();
                    _memoryCache.Remove(request.Email);
                    return new ErrorSuccessDTO<User>(user);
                }
                return new ErrorFailDTO<User>(ErrorCode.OTPIsNotCorrect);
            }
            return new ErrorFailDTO<User>(ErrorCode.OTPExpired);

        }
        
    }
}
