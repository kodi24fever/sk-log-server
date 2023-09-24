using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace SharkValleyServer.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string? UserName {get; set;} = "";

        [Required]
        public string? FirstName {get; set;}

        [Required]
        public string? LastName {get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; } = "";

        [Required]
        public string? Password { get; set; } = "";

    }
}