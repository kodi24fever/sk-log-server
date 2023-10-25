using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Data
{
    public class UserName
    {
        [Key]
        public int Id { get; set; }

        public string? Email { get; set;}

        public string? FirstName { get; set;}

        public string? LastName { get; set; }

    }
}