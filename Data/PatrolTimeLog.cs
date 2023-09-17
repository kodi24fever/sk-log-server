using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class PatrolTimeLog
    {
        [Key]
        public int Id { get; set; }

        public int? PatrolLogId { get; set; }
        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        public DateTime? PatrolDate { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? StartedPatrol { get; set; }
        public DateTime? CompletedPatrol { get; set; }
        public DateTime? LeftPatrol { get; set; }
    }
}
