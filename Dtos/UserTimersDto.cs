using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace SharkValleyServer.Dtos
{
    public class UserTimerDto
    {
        
        public DateTime LogInTime {get; set;}

        public DateTime StartedPatrolTime {get; set;}


        public DateTime EndedPatrolTime {get; set;}

        public DateTime LogILogOutTime {get; set;}


    }
}