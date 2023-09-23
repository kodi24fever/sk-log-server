using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;


using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;



namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserTimerController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;


        // Constructor. This is needed to initialize the objects to not null
        public UserTimerController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {

            // Get the api key from headers when user send post request
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized();


            
            return Ok("Hello To timers");

        }


        // Adding post method to api
        [HttpPost]

        public async Task<ActionResult> Post([FromBody] UserRegisterDto dto)
        {

            // Get the api key from headers when user send post request
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();



            // get current patrolLogID from Settings
            
            // getting body data
            string? username = dto.UserName;
            string? email = dto.Email;
            string? password = dto.Password;

  
            IdentityUser? user = await userManager.FindByEmailAsync(email);


            // var timerTable = await dbContext.Find(UserTimer, user.Email);



            user = new IdentityUser{UserName = username, Email = email};
            
            var result = await userManager.CreateAsync(user, password);

            return Ok(result);

        }

    }
}