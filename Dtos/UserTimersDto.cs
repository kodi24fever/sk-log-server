using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace SharkValleyServer.Dtos
{
    public class UserTimerDto
    {
        [Required]
        public int? PatrolLogId {get; set;}

        [Required]
        public string? Email { get; set; } = "";


        public DateTime LogInTime {get; set;}

        public DateTime StartedPatrolTime {get; set;}


        public DateTime EndedPatrolTime {get; set;}

        public DateTime LogILogOutTime {get; set;}


    }
}