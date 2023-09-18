using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;



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

            return Ok("Hello");

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
            
            user = new IdentityUser{UserName = username, Email = email};
            
            var result = await userManager.CreateAsync(user, password);
            

            return Ok(result);

        }

    }
}