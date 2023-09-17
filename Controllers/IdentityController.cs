using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManage)
        {
            this._userManager = userManager;
            this._signInManager = signInManage;
        }


        // POST api/<IdentityController>
        [HttpPost]
        public  async Task<IActionResult> Post([FromBody] UserLoginDto dto)
        {
            if(!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            IdentityUser? user = null;
            if(!string.IsNullOrEmpty(dto.Email) && !string.IsNullOrEmpty(dto.Password)) {
               var userFound = await _userManager.FindByEmailAsync(dto.Email);
                if (userFound != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(userFound, dto.Password, false);
                    if (signInResult.Succeeded)
                        user = userFound;
                }
            }
           
            if(user != null)
            {
                return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName });
            }

            return BadRequest("Incorrect Credentials");
          
        }


    }
}
