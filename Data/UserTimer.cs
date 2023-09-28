using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class UserTimer
    {
        [Key]
        public int Id { get; set; }

        public int? PatrolLogId { get; set; }
        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        public string? Email { get; set;}
        
        public DateTime LogInTime { get; set; }
        public DateTime StartedPatrolTime { get; set; }
        public DateTime EndedPatrolTime { get; set; }
        public DateTime LogOutTime { get; set; }


        public Boolean hasStartedPatrol { get; set; } = false;

        public Boolean hasEndedPatrol { get; set; } = false;

        public Boolean isCreator { get; set; } = false;

    }
}