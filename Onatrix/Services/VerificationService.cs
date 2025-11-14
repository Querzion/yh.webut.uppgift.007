using System.Diagnostics;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Caching.Memory;
using Onatrix.Models;
using Onatrix.Models.Results;
using Org.BouncyCastle.Asn1.Cmp;

namespace Onatrix.Services;

public interface IVerificationService
{
    Task<VerificationServiceResult> SendVerificationAsync(SendVerificationRequest request);
    Task SaveVerificationAsync(SaveVerificationRequest request);
    VerificationServiceResult VerifyVerification(VerifyVerificationRequest request);
}
public class VerificationService(IConfiguration configuration, EmailClient emailClient, IMemoryCache cache)
    : IVerificationService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly EmailClient _emailClient = emailClient;
    private readonly IMemoryCache _cache = cache;
    private static readonly Random _random = new();
    
    public async Task<VerificationServiceResult> SendVerificationAsync(SendVerificationRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return new VerificationServiceResult
                    { Succeeded = false, Error = "Recipient email address is required." };

            var randomNumber = _random.Next(100000, 999999).ToString();
            var siteAddress = "***";
            var siteLocation = "***";
            var subject = $"Your {siteLocation} request was delivered.";
            var plainTextContent = $@"
            You sent a ** request to us, on {siteAddress} 
            We will be contacting you as soon as possible.

            Thank you! Best regards Onatrix. . . . . . . . . . . . 
            Here is a random nr: {randomNumber}
        ";

            var htmlContent = $@"
            <!DOCTYPE html>
            <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <title>Your {siteLocation} request was delivered.</title>
                </head>
                <body>
                </body>
            </html>
        ";

            var emailMessage = new EmailMessage(
                senderAddress: _configuration["ACS:SenderAddress"],
                recipients: new EmailRecipients([new(request.Email)]),
                content: new EmailContent(subject)
                {
                    PlainText = plainTextContent,
                    Html = htmlContent
                });

            var emailSendOperation = await _emailClient.SendAsync(WaitUntil.Started, emailMessage);
            await SaveVerificationAsync(new SaveVerificationRequest
                { Email = request.Email, Verify = siteLocation, ValidFor = TimeSpan.FromMinutes(5) });

            return new VerificationServiceResult { Succeeded = true, Message = "Verification email sent successfully." };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new VerificationServiceResult { Succeeded = false, Error = "Failed to send verification email." };
        }
        
        
    }

    public Task SaveVerificationAsync(SaveVerificationRequest request)
    {
        _cache.Set(request.Email.ToLowerInvariant(), request.Verify, request.ValidFor);
        return Task.CompletedTask;
    }

    public VerificationServiceResult VerifyVerification(VerifyVerificationRequest request)
    {
        var key = request.Email.ToLowerInvariant();
        if (_cache.TryGetValue(key, out string? storedVerify))
        {
            if (storedVerify == request.Verify)
            {
                _cache.Remove(key);
                return new VerificationServiceResult { Succeeded = true, Message = "Verification successful." };
            }
        }

        return new VerificationServiceResult { Succeeded = false, Error = "Invalid or expired verification." };
    }
}