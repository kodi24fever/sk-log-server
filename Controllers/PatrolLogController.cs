using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SharkValleyServer.Data;
using SharkValleyServer.Dtos;
using SharkValleyServer.Services;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SharkValleyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatrolLogController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;


        public PatrolLogController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }



        // Return the last 10 logs created by a specific user
        [HttpGet("last10")]
        public async Task<IActionResult> Get()
        {
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);


            IdentityUser? user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(Request);
            }

            var userPatrolLogs = dbContext.PatrolLogs.Where(p => p.CreatedBy == user.UserName & p.WasCreated).OrderByDescending(p=>p.Created).Select(p => new {PatrolNo = p.PatrolNo, Created = p.Created}).Take(10).ToList();


            int userPatrolLogsCount = dbContext.PatrolLogs.Count(p => p.CreatedBy == user.UserName);
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            int patroLogsCount = int.Parse(patrolNoSetting?.Value??"0");
            
            return Ok(new { userPatrolLogs, userPatrolLogsCount, patroLogsCount });
        }


        // Get request to send info about initialized patrol
        [HttpGet("getInitLog")]
        public async Task<IActionResult> GetInitiLog()
        {
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);


            IdentityUser? user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(Request);
            }

            // PatrolNo cannot be increased everytime a request is made it has to be set by admin or increasded daily
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");


            int patrolNo = int.Parse(patrolNoSetting.Value);

            // get patrolLog for current patrolNo value
            var currentPatrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNoSetting.Value & pl.WasCreated == false).FirstOrDefaultAsync();


            if(currentPatrolLog == null){

                // / patrol log not found so it does not have creator either
                return new JsonResult(new { isCreated = false, isCreator = false, error = "Patrol not created"});

            }
            else if(currentPatrolLog != null && !currentPatrolLog.WasCreated)
            {
                //check if user is creator of patrol
                var currentUser = dbContext.UserTimers.Where(ut => ut.PatrolLogId == currentPatrolLog.Id & ut.Email == user.Email).FirstOrDefault();


                if(currentUser == null){
                    return new JsonResult(new { isCreated = true, isCreator = false, error = "user has not logged in to current patrol log"});
                }

                

                return new JsonResult(new { isCreated = true, isCreator = currentUser.isCreator, error = "log already created but not completed"});

            }

            // patrol log not found so it does not have creator either
            return new JsonResult(new { isCreated = false, isCreator = false, error = "Patrol not creatd"});
        }





        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PatrolLogDto dto)
        {
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);

            

            IdentityUser? user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized(Request);

            
            // Use this part to check if user is the one creating the log ?
            // if(!(await userManager.IsInRoleAsync(user, "Administrators")))
            // {
            //     return Unauthorized();
            // }

            await initializePatrolSettingIfNotExist();






            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");

            // get current patrolNo from db
            int patrolNo = int.Parse(patrolNoSetting.Value);

            // Get patrolLog here with current PatrolNo
            var patrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNoSetting.Value & pl.WasCreated == false).FirstOrDefaultAsync();


            // check if patrol log is null or does not exist
            if(patrolLog == null)
            {
                return Ok("You need to create a new instance of the patrol Log: " + patrolNoSetting.Value);
            }

            // check if patrol log is already created
            if(patrolLog.WasCreated == true)
            {
                return Ok("Patrol Log: " + patrolNoSetting.Value + " was already created");
            }


            // get Creator for TimerTables and check if a creator exists
            var checkCreator = await dbContext.UserTimers.Where(ut => ut.isCreator == true & patrolLog.Id == ut.PatrolLogId).FirstOrDefaultAsync();

            //return Ok("testing creation for logs creator if true " + checkCreator.Email + " for logNo: " + patrolLog.PatrolNo);

            // non creator users show unauthorized access
            if(checkCreator == null)
                return Unauthorized();


            if(checkCreator.Email == user.Email)
            {
                // no need for the patrolNo since we have it in initialization
                //patrolLog.PatrolNo = patrolNo.ToString();
                patrolLog.PatrolTime = dto.patrolTime;
                patrolLog.WeatherLog = dto.weatherLog;
                patrolLog.ContactLog = dto.contactLog;
                patrolLog.Comments = dto.comments;
                patrolLog.Signatures.AddRange(dto.signatures?.Select(s => new Signature { FullName = s }).ToList() ?? new List<Signature>());
                patrolLog.IncidentReports.AddRange(dto.incidentReports);
                patrolLog.WildLifeLogs.AddRange(dto.wildlifeSights?.Where(w=> w.Amount> 0).ToList()?? new List<WildLifeLog>());
                patrolLog.SupplyLogs.AddRange(dto.supplies);
                // we already know the creator when initialized the patrol log
                //patrolLog.CreatedBy = user.UserName;
                patrolLog.Created = DateTime.Now;
                patrolLog.WasCreated = true;


                // add changes and save them to db

                
                //await dbContext.AddAsync(patrolLog);
                //dbContext.SaveChanges();
                

                // Increase PatrolLog after creating one and save it in db
                patrolNo++;
                patrolNoSetting.Value = patrolNo.ToString();
                await dbContext.SaveChangesAsync();


                // Good idea to log Out all users after this line to track the end timer
                // only tracking time here

                var userEndTimers = dbContext.UserTimers.Where(ut => patrolLog.Id == ut.PatrolLogId);

                foreach(var user_timer in userEndTimers){
                    if(user_timer.EndedPatrolTime == DateTime.Parse("0001-01-01 00:00:00.0000000"))
                        user_timer.EndedPatrolTime = dto.endTime;
                        user_timer.hasEndedPatrol = true;
                }

                // save endTimers
                await dbContext.SaveChangesAsync();


                // Return Response
                return Ok(dto);
            }


            // return if none of the cases apply
            return Unauthorized();
        }



        // Initialization method for patrol logs
        // POST api/<ValuesController>
        [HttpPost("initPatrolLog")]
        public async Task<IActionResult> PostInitipatrolLog([FromBody] UserTimer userTimerDto)
        {
            // check API key
            if (!Auth.IsValidAPIKey(Request))
                return Unauthorized();

            // get userId
            string? userId = Auth.getUserId(Request);
            if (userId == null)
                return Unauthorized(Request);
            
            // get user object usign userId
            IdentityUser? user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized(Request);

            //Initialize patrolNo if it does not exist
            await initializePatrolSettingIfNotExist();


            // PatrolNo cannot be increased everytime a request is made it has to be set by admin or increasded daily
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            int patrolNo = int.Parse(patrolNoSetting.Value);



            // Check if db is empty and create first patrolLog
            // check if all the patrol logs exist
            var patrolLogsExist = dbContext.PatrolLogs.Any();


            // First User that logs in creates the log in case the db does not have previous logs
            if(!patrolLogsExist){

                // Initialize patrolLog Object
                PatrolLog newPatrolLog = new PatrolLog();

                newPatrolLog.PatrolNo = patrolNo.ToString();
                newPatrolLog.CreatedBy = user.UserName;
                newPatrolLog.HasCreator = true;

                await dbContext.AddAsync(newPatrolLog);
                dbContext.SaveChanges();


                // Initialize empty logIn timer
                UserTimer logIn = new UserTimer();

                // add data to timer of creator for the log
                logIn.PatrolLogId = newPatrolLog.Id;
                logIn.Email = user.Email;
                logIn.LogInTime = userTimerDto.LogInTime;
                logIn.isCreator = true;


                // save changes to db
                await dbContext.AddAsync(logIn);
                dbContext.SaveChanges();


                // Return Response
                return new JsonResult(new { CreatedBy = user.UserName, PatrloNo = patrolNoSetting.Value});

            }


            
            var currentPatrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNoSetting.Value & pl.WasCreated == false).FirstOrDefaultAsync();

            // handle error if currentPatrolLOg is null. COuld move lines 288 - 321 inside here to create a new patrolLog
            if(currentPatrolLog == null){

                //return Ok("Probably PatrolNo is already used. Contact Admin to chaneg patrolNo");
                // Initialize patrolLog Object
                PatrolLog patrolLog = new PatrolLog();

                // add data to new patrolLog
                patrolLog.PatrolNo = patrolNo.ToString();
                patrolLog.CreatedBy = user.UserName;
                patrolLog.HasCreator = true;

                // add user to userTimer and set isCreator to true


                // add changes and save them to db
                await dbContext.AddAsync(patrolLog);
                dbContext.SaveChanges();


                // Get userTimer Table
                var logExist = dbContext.UserTimers.Where(ut => ut.Email == user.Email & ut.PatrolLogId == patrolLog.Id).FirstOrDefault();


                // If logTimer does not exist create a new one
                if(logExist == null){

                    // Initialize empty logIn timer
                    UserTimer logIn = new UserTimer();

                    // add data to timer
                    logIn.PatrolLogId = patrolLog.Id;
                    logIn.Email = user.Email;
                    logIn.LogInTime = userTimerDto.LogInTime;
                    logIn.isCreator = true;

                    // save creator to db
                    await dbContext.AddAsync(logIn);
                    dbContext.SaveChanges();
                }


                // Return Response
                return new JsonResult(new { CreatedBy = user.UserName, PatrloNo = patrolNoSetting.Value});
            }


            //Console.WriteLine(currentPatrolLog.Id);


            if(currentPatrolLog != null && !currentPatrolLog.WasCreated)
            {
                return new JsonResult(new { succeded = false, error = "log already created but not completed"});

            }



            // Return Response
            return new JsonResult(new { CreatedBy = "no user", PatrloNo = "No #"});



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
