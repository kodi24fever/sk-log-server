using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class UserRegisterDto
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
