using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class WeatherLog
    {

        [Key]
        public int Id { get; set; }

        public int? PatrolLogId { get; set; }
        [JsonIgnore]
        public PatrolLog? PatrolLog { get; set; }

        public  int? Temperature {get; set;}
       public string? Wind { get; set; }
        public string? CloudCover { get; set; }
        public int? Humidity { get; set; }
        public string? Comments { get; set; }
    }
}
