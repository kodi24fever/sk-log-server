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
                return NotFound();
                // return Unauthorized(Request);

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


            await initializePatrolSettingIfNotExist();

            var patrolNoSetting = await dbContext.Settings.FindAsync("PatrolNo");
            int patrolNo = int.Parse(patrolNoSetting.Value);
            patrolNo++;
            patrolNoSetting.Value = patrolNo.ToString();
            await dbContext.SaveChangesAsync();

            PatrolLog patrolLog = new PatrolLog();

            
            patrolLog.PatrolNo = patrolNo.ToString();
            patrolLog.PatrolTime = dto.patrolTime;
            patrolLog.WeatherLog = dto.weatherLog;
            patrolLog.ContactLog = dto.contactLog;
            patrolLog.Comments = dto.comments;
            patrolLog.IncidentReports.AddRange(dto.incidentReports);
            patrolLog.WildLifeLogs.AddRange(dto.wildlifeSights?.Where(w=> w.Amount> 0).ToList()?? new List<WildLifeLog>());
            patrolLog.SupplyLogs.AddRange(dto.supplies);
            patrolLog.Signatures.AddRange(dto.signatures?.Select(s => new Signature { FullName = s }).ToList() ?? new List<Signature>());
            patrolLog.CreatedBy = user.UserName;
            patrolLog.Created = DateTime.Now;
            await dbContext.AddAsync(patrolLog);
            dbContext.SaveChanges();
            return Ok(dto);
        }


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
