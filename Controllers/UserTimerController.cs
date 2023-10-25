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


        // start patrol timer post method api
        [HttpPost("startTime")]
        public async Task<ActionResult> PostStartPatrolTimer([FromBody] UserTimerDto dto)
        {

            // Get the api key from headers when user send post request
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();


            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);

            
            // verify users
            IdentityUser?  user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();



            // get current patrolLogID from Settings
            var currentPatrolLogId = await dbContext.Settings.FindAsync("PatrolNo");

            // Check patrolLog value for nullable
            if(currentPatrolLogId == null){
                return new JsonResult(new { hasEndedPatrol = false, error = "Patrol Does not Exist Contact Manager"});
            }else{

                if(currentPatrolLogId.Value == null){
                    return new JsonResult(new { hasEndedPatrol = false, error = "Patrol Log Has not been assigned. Contact Manager"});
                }
            }

            // get current patrolLog for the specified patrolNo
            var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == currentPatrolLogId.Value.ToString()).FirstOrDefault();

            // check if patrolLog exists and it is initialized
            if(patrolLog == null){

                return new JsonResult(new { hasStartedPatrol = false, error = "Patrol not initialized"});
            }


            // check if patrolLogId and email exist in the table return bad repsonse else add start timer
            var logExist = dbContext.UserTimers.Where(ut => ut.Email == user.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();

            // check if logs exists in userTimers
            if(logExist == null)
            {
                return new JsonResult(new { hasStartedPatrol = false, error = "Patrol not initialized or does not exist"});

            }

            // log already exists
            if(logExist.hasStartedPatrol == true)
            {
                //handle return if user hasStartedPatrol
                return new JsonResult(new { hasStartedPatrol = false, error = "Sorry already startedPatrol"});
            }
            else
            {
            
                // getting body data
                logExist.StartedPatrolTime = dto.StartedPatrolTime;

                logExist.hasStartedPatrol = true;

                // save startedPatrolTime to db
                dbContext.SaveChanges();


                return new JsonResult(new { hasStartedPatrol = true, error = "Timer Saved"});

            }
        }


        // end patrol timer post method api
        [HttpPost("endTime")]
        public async Task<ActionResult> PostEndPatrolTimer([FromBody] UserTimerDto dto)
        {

            // Get the api key from headers when user send post request
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();


            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);

            
            // verify users
            IdentityUser?  user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();



            // get current patrolLogID from Settings
            var currentPatrolLogId = await dbContext.Settings.FindAsync("PatrolNo");

            // Check patrolLog value for nullable
            if(currentPatrolLogId == null){
                return new JsonResult(new { hasEndedPatrol = false, error = "Patrol Does not Exist Contact Manager"});
            }else{

                if(currentPatrolLogId.Value == null){
                    return new JsonResult(new { hasEndedPatrol = false, error = "Patrol Log Has not been assigned. Contact Manager"});
                }
            }


            // get current patrolLog for the specified patrolNo
            var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == currentPatrolLogId.Value.ToString()).FirstOrDefault();

            if(patrolLog == null){

                return new JsonResult(new { hasEndedPatrol = false, error = "Patrol not initialized"});
            }


            // check if patrolLogId and email exist in the table rturn bad repsonse else add start timer
            var logExist = dbContext.UserTimers.Where(ut => ut.Email == user.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


            if(logExist == null)
            {
                //handle return if user hasStartedPatrol
                return new JsonResult(new { hasEndedPatrol = false, error = "Patrol not initialized or does not exist"});

            }



            if(logExist.hasEndedPatrol == true)
            {
                //handle return if user hasStartedPatrol

                return new JsonResult(new { hasEndedPatrol = false, error = "Sorry already EndedPatrol"});
            }
            else
            {
            
                // getting body data
                logExist.EndedPatrolTime = dto.EndedPatrolTime;

                logExist.hasEndedPatrol = true;

                // save startedPatrolTime to db
                dbContext.SaveChanges();



                return new JsonResult(new { hasEndedPatrol = true, error = "Timer Saved"});

            }
        }

    }
}