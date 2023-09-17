using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class WildLifeLog
    {
        public int? PatrolLogId { get; set; }
        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        [Key]
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? CreatedBy{ get; set; }    
        public int? Amount { get; set; }
        public DateTime Created { get; set; }
    }
}
