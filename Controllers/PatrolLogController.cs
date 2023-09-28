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
                
            
            // Check that use is admin before creating a patrol log
            if(!(await userManager.IsInRoleAsync(user, "Administrators")))
            {
                return Unauthorized();
            }

            var userPatrolLogs = dbContext.PatrolLogs.Where(p => p.CreatedBy == user.UserName).OrderByDescending(p=>p.Created).Select(p => new {PatrolNo =p.PatrolNo, Created = p.Created}).Take(10).ToList();
            int userPatrolLogsCount = dbContext.PatrolLogs.Count(p => p.CreatedBy == user.UserName);
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            int patroLogsCount = int.Parse(patrolNoSetting?.Value??"0");
            
            return Ok(new { userPatrolLogs, userPatrolLogsCount, patroLogsCount });
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
            // Increase patrol log everytime a new one is created


            // Get patrolLog here with current PatrolNo
            var patrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNoSetting.Value).FirstOrDefaultAsync();


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

            
            //patrolLog.PatrolNo = patrolNo.ToString();
            patrolLog.PatrolTime = dto.patrolTime;
            patrolLog.WeatherLog = dto.weatherLog;
            patrolLog.ContactLog = dto.contactLog;
            patrolLog.Comments = dto.comments;
            patrolLog.Signatures.AddRange(dto.signatures?.Select(s => new Signature { FullName = s }).ToList() ?? new List<Signature>());
            patrolLog.IncidentReports.AddRange(dto.incidentReports);
            patrolLog.WildLifeLogs.AddRange(dto.wildlifeSights?.Where(w=> w.Amount> 0).ToList()?? new List<WildLifeLog>());
            patrolLog.SupplyLogs.AddRange(dto.supplies);
            //patrolLog.CreatedBy = user.UserName;
            patrolLog.Created = DateTime.Now;
            patrolLog.WasCreated = true;


            // add changes and save them to db

            
            //await dbContext.AddAsync(patrolLog);
            dbContext.SaveChanges();
            

            // Increase PatrolLog
            patrolNo++;
            patrolNoSetting.Value = patrolNo.ToString();
            await dbContext.SaveChangesAsync();


            // Return Response
            return Ok(dto);
        }
        
        
        
        // POST api/<ValuesController>
        [HttpPost("initPatrolLog")]
        public async Task<IActionResult> PostInitipatrolLog()
        {
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


            // No need to increase it on initialization
            //patrolNo++;
            // patrolNoSetting.Value = patrolNo.ToString();
            // await dbContext.SaveChangesAsync();

            
            var currentPatrolLog = await dbContext.PatrolLogs.Where(pl => pl.PatrolNo == patrolNoSetting.Value).FirstOrDefaultAsync();


            if(currentPatrolLog != null && !currentPatrolLog.WasCreated)
            {
                return Ok("An instance was created but not completed");

            }



            // Initialize patrolLog Object
            PatrolLog patrolLog = new PatrolLog();

            
            patrolLog.PatrolNo = patrolNo.ToString();
            patrolLog.CreatedBy = user.UserName;
            patrolLog.HasCreator = true;

            // add changes and save them to db
            await dbContext.AddAsync(patrolLog);
            dbContext.SaveChanges();


            // Return Response

            return new JsonResult(new { CreatedBy = user.UserName, PatrloNo = patrolNoSetting.Value});

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
