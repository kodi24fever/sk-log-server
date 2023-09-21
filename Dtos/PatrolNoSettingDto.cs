using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class PatrolNoSettingDto
    {
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
    }
}
