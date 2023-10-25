using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string? FirstName {get; set;} = "";

        [Required]
        public string? LastName {get; set; } = "";

        [EmailAddress]
        [Required]
        public string? Email { get; set; } = "";

        [Required]
        public string? Password { get; set; } = "";
    }
}
