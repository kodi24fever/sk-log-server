using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;
using SharkValleyServer.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ApplicationDbContext dbContext;

        public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManage, ApplicationDbContext dbContext)
        {
            this._userManager = userManager;
            this._signInManager = signInManage;
            this.dbContext = dbContext;
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
                    {
                        user = userFound;

                        // Add Timer on sign in
                        // get current patrolLogID from Settings
                        var patrolNo = await dbContext.Settings.FindAsync("PatrolNo");
                        
                        // get current patrolLog for the specified patrolNo
                        var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNo.Value.ToString()).FirstOrDefault();

                        if(patrolLog == null){
                            Console.WriteLine("No Object");

                        }else{

                        Console.WriteLine(patrolLog.Id);


                        // Initialize empty logIn timer
                        UserTimer logIn = new UserTimer();

                        logIn.PatrolLogId = patrolLog.Id;
                        logIn.Email = user.Email;
                        logIn.LogInTime = DateTime.Now;


                        // save changes to db
                        await dbContext.AddAsync(logIn);
                        dbContext.SaveChanges();

                        }

                    }
                        
                }
            }



            if(user != null)
            {

                // we can insert timer here before return response of users logged in

                return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName });
            }

            return BadRequest("Incorrect Credentials");
          
        }


    }
}
