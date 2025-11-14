using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Onatrix.Models;
using Onatrix.Services;

namespace Onatrix.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationController(IVerificationService verificationService) : ControllerBase
{
    private readonly IVerificationService _verificationService = verificationService;

    [HttpPost("send")]
    public async Task<IActionResult> Send(SendVerificationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Recipient email address is required." });

        var result = await _verificationService.SendVerificationAsync(request);
        return result.Succeeded
            ? Ok(result)
            : StatusCode(500, result);
    }

    [HttpPost("verify")]
    public IActionResult Verify(VerifyVerificationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Error = "Invalid or expired verification." });

        var result = _verificationService.VerifyVerification(request);
        return result.Succeeded
            ? Ok(result)
            : StatusCode(500, result);
    }
}