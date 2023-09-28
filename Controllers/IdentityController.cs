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
        [HttpPost("logIn")]
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

                    }   
                }
            }



            if(user != null)
            {

                // Set timer here before return response of users logged in

                // Add Timer on sign in
                // get current patrolLogID from Settings
                var patrolNo = await dbContext.Settings.FindAsync("PatrolNo");

                
                // get current patrolLog for the specified patrolNo
                var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNo.Value.ToString()).FirstOrDefault();

                // current role
                var role = "Administrators";


                // if patrolLog object is initialized and not created then submit log in timer 
                if(patrolLog != null && patrolLog.WasCreated == true){

                    // Get userTimer Table
                    var logExist = dbContext.UserTimers.Where(ut => ut.Email == dto.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


                    // If logTimer does not exist create a new one
                    if(logExist == null){

                        // Initialize empty logIn timer
                        UserTimer logIn = new UserTimer();

                        // add data to timer
                        logIn.PatrolLogId = patrolLog.Id;
                        logIn.Email = user.Email;
                        logIn.LogInTime = DateTime.Now;


                        // save changes to db
                        await dbContext.AddAsync(logIn);
                        dbContext.SaveChanges();
                    }


                    // return json response in json with respectives roles
                    if(await _userManager.IsInRoleAsync(user, role)){
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "Admin"});
                    }
                    else{
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "User"});
                    }
                }
                else 
                {

                    // returns response if patrol log is not initiated or already created
                    return new JsonResult(new { succeeded = false, error = "patrol log not created"});
                }

                
            }

            // repsonse if user does not exist
            return BadRequest("Incorrect Credentials");
          
        }




        [HttpPost("logOut")]
        public async Task<ActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok("successfully signed out");
        }

    }
}
