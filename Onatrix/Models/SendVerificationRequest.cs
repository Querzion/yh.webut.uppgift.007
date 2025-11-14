using System.ComponentModel.DataAnnotations;

namespace Onatrix.Models;

public class SendVerificationRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    public string? SiteAddress { get; set; }
    public string? SiteLocation { get; set; }
    public string? FormType { get; set; }
    
    public string? Message { get; set; }
}