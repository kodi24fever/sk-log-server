using System.Security.Cryptography.X509Certificates;
using SharkValleyServer.Data;

namespace SharkValleyServer.Dtos
{
    public class PatrolLogDto
    {
      public  PatrolTimeLog? patrolTime { get; set; }
      public WeatherLog? weatherLog { get; set; }

      public  ContactLog? contactLog { get; set; }

      public  String? comments { get; set; }

      public  List<IncidentReport>? incidentReports { get; set; }

      public  List<WildLifeLog>? wildlifeSights { get; set; }

      public  List<SupplyLog>? supplies { get; set; }

      public List<String>? signatures { get; set; }

      // this timer can be used as logout and end patrol timers
      public DateTime endTime { get; set;}


    }
}
