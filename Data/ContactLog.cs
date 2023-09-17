using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class ContactLog
    {
        [Key]
        public int Id { get; set; }

        public int? PatrolLogId { get; set; }

        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        public int NoContacts { get; set; }
        public String? Comments { get; set; }
    }
}
