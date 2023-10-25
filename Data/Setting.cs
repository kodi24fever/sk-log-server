using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharkValleyServer.Data
{
    public class Setting
    {
        [Key]
        public string? Key { get; set; }
        public string? Value { get; set; }

    }
}
