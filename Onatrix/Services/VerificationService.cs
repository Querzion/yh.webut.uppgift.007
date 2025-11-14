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
            var siteAddress = request.SiteAddress ?? "Unknown site";
            var siteLocation = request.SiteLocation ?? "Unknown location";
            var formType = request.FormType ?? "Form submission";
            var messageBody = request.Message ?? "(No additional message provided)";

            
            var subject = $"Your {formType} request was delivered.";
            
            var plainTextContent = $@"
                You sent a '{formType}' to us, on {siteAddress} 
                Regarding:
                {messageBody}

                We will be contacting you as soon as possible.

                Thank you! Best regards Onatrix. . . . . . . . . . . . 
                Here is a random nr: {randomNumber}
            ";

            var htmlContent = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>{formType} received</title>
                <style>
                    body {{
                        font-family: 'Poppins', sans-serif;
                        background-color: #F7F7F7; /* var(--color-white-100) */
                        color: #535656; /* var(--color-body-text) */
                        line-height: 1.6;
                        margin: 0;
                        padding: 20px;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        border: 1px solid #EAEAEA; /* var(--color-border-gray) */
                        padding: 20px;
                        border-radius: 8px;
                        background-color: #FFFFFF; /* var(--color-neutral) */
                    }}
                    .header {{
                        font-size: 24px;
                        font-weight: 600;
                        color: #4F5955; /* var(--color-primary) */
                        margin-bottom: 12px;
                    }}
                    .message {{
                        background-color: #F2EDDC; /* var(--color-sub1) */
                        padding: 12px;
                        border-radius: 6px;
                        margin: 10px 0;
                    }}
                    .footer {{
                        color: #999999; /* var(--color-body-text-100) */
                        margin-top: 24px;
                        font-size: 14px;
                    }}
                    .code {{
                        font-size: 22px;
                        font-weight: bold;
                        color: #0E0E0E; /* var(--color-dark) */
                        letter-spacing: 3px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>You sent a '<strong>{formType}</strong>' to us</div>

                    <p>
                        From: <strong>{System.Net.WebUtility.HtmlEncode(siteAddress)}</strong>
                    </p>

                    <p><strong>Regarding:</strong></p>
                    <div class='message'>
                        {System.Net.WebUtility.HtmlEncode(messageBody).Replace("\n", "<br>")}
                    </div>

                    <p>
                        We will be contacting you as soon as possible.
                    </p>

                    <hr style='border-color: #C7C8C9;'> <!-- var(--color-border-400) -->

                    <p class='footer'>
                        Thank you! Best regards,<br>
                        <strong>Onatrix</strong>
                    </p>

                    <p>
                        Here is a random nr:<br>
                        <span class='code'>{randomNumber}</span>
                    </p>
                </div>
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
                { Email = request.Email, Verify = formType, ValidFor = TimeSpan.FromMinutes(5) });

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