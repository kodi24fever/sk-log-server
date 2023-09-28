using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;




using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Net.WebSockets;



namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;


        // Constructor. This is needed to initialize the objects to not null
        public RegisterController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {

            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);


            IdentityUser?  user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
                // return Unauthorized(Request);



            var isInRole = await userManager.IsInRoleAsync(user, "Administrators");


            if(isInRole){
                return Ok("Admin");
            }
            return Ok("Not Admin");

        }


        // Adding post method to api
        [HttpPost]

        public async Task<ActionResult> Post([FromBody] UserRegisterDto dto)
        {

            // Get the api key from headers when user send post request
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            
            // getting body data
            string? username = dto.UserName;
            string? email = dto.Email;
            string? password = dto.Password;

  
            IdentityUser? user = await userManager.FindByEmailAsync(email);


            // check if user is null to create a new one
            if(user == null)
            {
                user = new IdentityUser{UserName = username, Email = email};
                
                var result = await userManager.CreateAsync(user, password);

                return Ok(result);

            }
            
            
            
            return new JsonResult(new { succeeded = false, error = "user exists"});
            

        }

    }
}