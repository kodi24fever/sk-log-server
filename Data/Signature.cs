using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    // Signature can be understood as Users Signature
    public class Signature
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }

        public int? PatrolLogId { get; set; }  // Foreign Key
        public PatrolLog? PatrolLog { get; set; } // Required reference navigator not null
    }
}
