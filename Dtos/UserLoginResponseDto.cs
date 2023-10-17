namespace SharkValleyServer.Dtos
{
    public class UserLoginResponseDto
    {
       public string? Id { get; set; }
       public string? Email { get; set; }
       public  string? UserName { get; set; }

       public  string? Role { get; set; }

       public Boolean? IsPatrolLogCreated { get; set; }

    }
}
