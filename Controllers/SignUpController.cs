using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;

namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignUpController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManage)
        {
            this.dbContext = dbContext;
            this._userManager = userManager;
            this._signInManager = signInManage;
        }


        // POST api/<IdentityController>
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(UserRegisterDto dto)
        {


            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();


            if(dto.Email == null){

                return BadRequest("Wrong Email");
            }


            // getting body data
            string? username = dto.Email; // this part can be changed to recieve UserName from front end
            string? email = dto.Email;
            string? password = dto.Password;


            var userFound = await _userManager.FindByEmailAsync(dto.Email);


            if(userFound != null)
            {
                return BadRequest("Account with the given email already exists!");
            }


            if(userFound == null){

                // Creates a new user based on the details sent to this method
                var NewUser = new IdentityUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(NewUser, dto.Password);

                // if user was succesfully created in the aspnetuser table
                if(result.Succeeded){

                    var userName = dbContext.UserName.Where(un => un.Email == dto.Email).FirstOrDefault();

                    UserName userFullName = new UserName();

                    userFullName.Email = dto.Email;
                    userFullName.FirstName = dto.FirstName;
                    userFullName.LastName = dto.LastName;

                    // add new user firstname and lastname to UserName Table
                    await dbContext.AddAsync(userFullName);
                    dbContext.SaveChanges();

                }


                return Ok(result);
            }


            return Ok();
            
        }
    }
}
