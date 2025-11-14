using System.ComponentModel.DataAnnotations;

namespace Onatrix.Models;

public class SendVerificationRequest
{
    [Required]
    public string Email { get; set; } = null!;
}