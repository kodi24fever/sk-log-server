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

            
            // Check that use is admin before creating a patrol log
            if(!(await userManager.IsInRoleAsync(user, "Administrators")))
            {
                return Unauthorized();
            }

            await initializePatrolSettingIfNotExist();


            // PatrolNo cannot be increased everytime a request is made it has to be set by admin or increasded daily
            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            int patrolNo = int.Parse(patrolNoSetting.Value);
            // patrolNo++;
            patrolNoSetting.Value = patrolNo.ToString();
            await dbContext.SaveChangesAsync();


            // Initialize patrolLog Object
            PatrolLog patrolLog = new PatrolLog();

            
            patrolLog.PatrolNo = patrolNo.ToString();
            patrolLog.PatrolTime = dto.patrolTime;
            patrolLog.WeatherLog = dto.weatherLog;
            patrolLog.ContactLog = dto.contactLog;
            patrolLog.Comments = dto.comments;
            patrolLog.Signatures.AddRange(dto.signatures?.Select(s => new Signature { FullName = s }).ToList() ?? new List<Signature>());
            patrolLog.IncidentReports.AddRange(dto.incidentReports);
            patrolLog.WildLifeLogs.AddRange(dto.wildlifeSights?.Where(w=> w.Amount> 0).ToList()?? new List<WildLifeLog>());
            patrolLog.SupplyLogs.AddRange(dto.supplies);
            patrolLog.CreatedBy = user.UserName;
            patrolLog.Created = DateTime.Now;


            // add changes and save them to db
            await dbContext.AddAsync(patrolLog);
            dbContext.SaveChanges();


            // Return Response
            return Ok(dto);
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
