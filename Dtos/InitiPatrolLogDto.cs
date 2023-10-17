using SharkValleyServer.Data;

namespace SharkValleyServer.Dtos
{
    public class InitPatrolLogDto
    {
      public  PatrolTimeLog? patrolTime { get; set; }
      public WeatherLog? weatherLog { get; set; }

      public  ContactLog? contactLog { get; set; }

      public  String? comments { get; set; }

      public  List<IncidentReport>? incidentReports { get; set; }

      public  List<WildLifeLog>? wildlifeSights { get; set; }

      public  List<SupplyLog>? supplies { get; set; }

      public List<String>? signatures { get; set; }
    }
}