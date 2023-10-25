using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class UserLoginDto
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; } = "";

        [Required]
        public string? Password { get; set; } = "";

        [Required]
        public DateTime LogInTime {get; set;}

    }
}
