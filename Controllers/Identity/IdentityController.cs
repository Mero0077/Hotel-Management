using Hotel_Management.Helper;
using Hotel_Management.Models.ViewModels.Account;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
       private UserService _userService;
        public IdentityController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync (LoginVM loginVM)
        {
            var user= await _userService.Login(loginVM.UserName, loginVM.Password);
            if (user.Id == 0)
            {
                return Unauthorized("Invalid Username or Pass!");
            }

            string token = Helper.GenerateToken.Generate(user.Id, user.FullName,user.Role);
            return Ok(new {Token=token});
        }
    }
}
