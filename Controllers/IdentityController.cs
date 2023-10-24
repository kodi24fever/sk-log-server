using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;
using SharkValleyServer.Data;
using Microsoft.EntityFrameworkCore;

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
            
            if(!string.IsNullOrEmpty(dto.Email) & !string.IsNullOrEmpty(dto.Password)) {
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


            // check if user exists
            if(user != null)
            {

                // Set timer here before return response of users logged in

                // Add Timer on sign in
                // get current patrolLogID from Settings
                var patrolNo = dbContext.Settings.Find("PatrolNo");


                // current role
                var role = "Administrators";

                // initialize patrolNo if it does not exist
                if(patrolNo == null){

                    await initializePatrolSettingIfNotExist();

                    // Get the initialized patrolNo
                    patrolNo = dbContext.Settings.Find("PatrolNo");

                }else{

                    if(patrolNo.Value == null){
                        return new JsonResult(new { error = "No Patrol No contact manager"});
                    }
                }

                

                // get current patrolLog for the specified patrolNo
                var patrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNo.Value.ToString()).FirstOrDefaultAsync();



                // if patrolLog object is initialized and not created then submit log in timer 
                if(patrolLog != null && !patrolLog.WasCreated){

                    // Get userTimer Table
                    var logExist = dbContext.UserTimers.Where(ut => ut.Email == dto.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


                    // If logTimer does not exist create a new one. Probvably don't need the if statement because logTimer does not exist
                    if(logExist == null){

                        // Initialize empty logIn timer
                        UserTimer logIn = new UserTimer();

                        // add data to timer of creator for the log
                        logIn.PatrolLogId = patrolLog.Id;
                        logIn.Email = user.Email;
                        logIn.LogInTime = dto.LogInTime;


                        // save changes to db
                        await dbContext.AddAsync(logIn);
                        dbContext.SaveChanges();
                    }


                    // return json response in json with respectives roles when patrol log is created
                    if(await _userManager.IsInRoleAsync(user, role)){
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "Admin", IsPatrolLogCreated = true});
                    }
                    else{
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "User", IsPatrolLogCreated = true});
                    }
                }
                else 
                {

                    // return json response in json with respectives roles return json response in json with respectives roles when patrol log IS NOT created
                    if(await _userManager.IsInRoleAsync(user, role)){
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "Admin", IsPatrolLogCreated = false});
                    }
                    else{
                        return new JsonResult(new UserLoginResponseDto { Id = user.Id, Email = user.Email, UserName = user.UserName, Role = "User", IsPatrolLogCreated = false});
                    }
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



        // it creates a default PatrolNo in Settings table if it does not exist
        private async Task initializePatrolSettingIfNotExist()
        {
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            if (patrolNoSetting == null)
            {
                patrolNoSetting = new Setting { Key = "PatrolNo", Value = "4000" };
                dbContext.Add(patrolNoSetting);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
