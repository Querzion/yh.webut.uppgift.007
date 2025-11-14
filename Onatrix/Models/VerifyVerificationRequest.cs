using System.ComponentModel.DataAnnotations;

namespace Onatrix.Models;

public class VerifyVerificationRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Verify { get; set; } = null!;
}