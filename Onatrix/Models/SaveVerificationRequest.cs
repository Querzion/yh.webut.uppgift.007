using System.ComponentModel.DataAnnotations;

namespace Onatrix.Models;

public class SaveVerificationRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Verify { get; set; } = null!;
    
    public TimeSpan ValidFor { get; set; }
}