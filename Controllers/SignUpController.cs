using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;

namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignUpController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManage)
        {
            this._userManager = userManager;
            this._signInManager = signInManage;
        }


        // POST api/<IdentityController>
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto dto)
        {
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            var userFound = await _userManager.FindByEmailAsync(dto.Email);
            if(userFound != null)
            {
                return BadRequest("Account with the given email already exists!");
            }

            // Creates a new user based on the details sent to this method
            var NewUser = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                EmailConfirmed = true,
            };
            await _userManager.CreateAsync(NewUser, dto.Password);
            return Ok();

            
        }
    }
}
