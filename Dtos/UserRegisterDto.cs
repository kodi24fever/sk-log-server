using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class UserRegisterDto
    {

        public string? UserName { get; set;} = "";
        [EmailAddress]
        [Required]
        public string? Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
