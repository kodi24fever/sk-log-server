using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class SupplyLog
    {
        public int? PatrolLogId { get; set; }
        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        [Key]
        public int Id { get; set; }
        public string? Type { get; set; }
        public int  Number { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime created { get; set; }
    }
}
