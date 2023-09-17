using System.ComponentModel.DataAnnotations;

namespace SharkValleyServer.Dtos
{
    public class PatrolLogEditDto
    {
      public int Id { get; set; }

      [Required]
      public string? PatrolNo { get; set; }

      [Required]
      public string? Comments { get; set; }

      public string? LastUpdate { get; set; }

      public string? UpdatedBy { get; set; }
  
    }
}
