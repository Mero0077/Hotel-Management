using Hotel_Management.DTOs.Account;
using Hotel_Management.DTOs.User;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Services
{
    public class UserService
    {
        GeneralRepository<User> _generalRepository;
        public UserService(GeneralRepository<User>  generalRepository) 
        { 
            _generalRepository = generalRepository;
        }

        public async Task<UserLoginDTO> Login(string username, string password)
        {
            // hashing for both inputs ?????
          var res= await _generalRepository.Get(u=>u.UserName==username && u.Password==password).Select(u=>new UserLoginDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Role = u.Role,
               
            }).FirstOrDefaultAsync();

            return res?? new UserLoginDTO{ Id=0,Role=Role.None};
        }
    }
}
