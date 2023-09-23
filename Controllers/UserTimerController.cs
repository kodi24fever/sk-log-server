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

            // get current patrolLog for the specified patrolNo
            var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == currentPatrolLogId.Value.ToString()).FirstOrDefault();


            // check if patrolLogId and email exist in the table rturn bad repsonse else add start timer
            var logExist = dbContext.UserTimers.Where(ut => ut.Email == dto.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


            Console.WriteLine(logExist.Email);


            if(logExist == null)
            {
                Console.WriteLine("it does not exist log");


            }else{
                Console.WriteLine("it exists log");
            }



            if(logExist.hasStartedPatrol == true)
            {
                //handle return if user hasStartedPatrol

                return Ok("Soryy already startedPatrol");
            }
            else
            {
            
                // getting body data
                logExist.StartedPatrolTime = dto.StartedPatrolTime;

                logExist.hasStartedPatrol = true;

                // save startedPatrolTime to db
                dbContext.SaveChanges();

            }
            return Ok("Timer Started");
        }


        // end patrol timer post method api
        [HttpPost("endTimer")]
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

            // get current patrolLog for the specified patrolNo
            var patrolLog = dbContext.PatrolLogs.Where(pl => pl.PatrolNo == currentPatrolLogId.Value.ToString()).FirstOrDefault();


            // check if patrolLogId and email exist in the table rturn bad repsonse else add start timer
            var logExist = dbContext.UserTimers.Where(ut => ut.Email == dto.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


            if(logExist == null)
            {
                Console.WriteLine("it does not exist log");


            }else{
                Console.WriteLine("it exists log");
            }



            if(logExist.hasEndedPatrol == true)
            {
                //handle return if user hasStartedPatrol

                return Ok("Sorry You Already Ended Patrol");
            }
            else
            {
            
                // getting body data
                logExist.EndedPatrolTime = dto.EndedPatrolTime;

                logExist.hasEndedPatrol = true;

                // save startedPatrolTime to db
                dbContext.SaveChanges();

            }
            return Ok("Patrol Timer Ended");
        }




    }
}