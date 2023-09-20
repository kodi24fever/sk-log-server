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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<PatrolLog>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PatrolLog>()
                .HasMany(e => e.Signatures)
                .WithOne(e => e.PatrolLog)
                .HasForeignKey(e => e.PatrolLogId)
                .IsRequired();
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



    }
}