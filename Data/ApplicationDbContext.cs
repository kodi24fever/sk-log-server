using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace SharkValleyServer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<PatrolTimeLog> PatrolTimeLogs { get; set; }
        public DbSet<WeatherLog> WeatherLogs { get; set; }
        public DbSet<ContactLog> ContactLogs { get; set; }
        public DbSet<IncidentReport> IncidentReports { get; set; }
        public DbSet<WildLifeLog> WildLifeLogs { get; set; }
        public DbSet<SupplyLog> SupplyLogs { get; set; }
        public DbSet<PatrolLog> PatrolLogs { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<Setting> Settings { get; set; }


        // Set new models here to make chanegs in database
        public DbSet<UserTimer> UserTimers { get; set; }


        public DbSet<UserName> UserName { get; set; }


    }
}