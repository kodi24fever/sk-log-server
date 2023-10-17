using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Data
{
    public class PatrolLog
    {
        [Key]
        public int Id { get; set; }
        public string? PatrolNo { get; set; }
        public  PatrolTimeLog? PatrolTime { get; set; }
        public  WeatherLog? WeatherLog { get; set; }
        public  ContactLog? ContactLog { get; set; }
        public String? Comments { get; set; }
        
        public IList<IncidentReport> IncidentReports { get; } = new List<IncidentReport>(); // Collection navigation

        public IList<WildLifeLog> WildLifeLogs { get; } = new List<WildLifeLog>(); // Collection navigation

        public IList<SupplyLog> SupplyLogs { get; } = new List<SupplyLog>(); // Collection navigation

        public IList<Signature> Signatures { get; } = new List<Signature>(); // Collection navigation

        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime LastUpdate { get; set; }

        public string? UpdatedBy { get; set; }

        public Boolean HasCreator { get; set; } = false;

        public Boolean WasCreated { get; set; } = false; 

       
    }
}
