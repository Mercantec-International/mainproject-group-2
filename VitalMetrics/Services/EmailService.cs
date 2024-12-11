using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _templatePath;

    public EmailService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _templatePath = Path.Combine(
            environment.ContentRootPath,

            "Templates",
            "EmailConfirmation.html"
        );
    }

    private async Task<string> GetEmailTemplate(string confirmationUrl)
    {
        if (!File.Exists(_templatePath))
        {
            throw new FileNotFoundException($"Email template file not found at {_templatePath}");
        }

        using var reader = new StreamReader(_templatePath);
        string template = await reader.ReadToEndAsync();
        return template.Replace("{confirmationUrl}", confirmationUrl);
    }

    public async Task SendConfirmationEmail(string email, string emailConfirmationToken)
    {
        try
        {
            // Generate confirmation URL
            string confirmationToken = Guid.NewGuid().ToString();
            var confirmationUrl = $"https://mainproject-group-2.onrender.com/api/User/confirm-email?token={confirmationToken}&email={email}";
            var emailBody = await GetEmailTemplate(confirmationUrl);

            // Fetch SMTP credentials from configuration or environment variables
            var smtpEmail =  Environment.GetEnvironmentVariable("EmailService:Email");
            var smtpPassword = Environment.GetEnvironmentVariable("EmailService:Password");

            // Validate SMTP credentials
            if (string.IsNullOrWhiteSpace(smtpEmail))
            {
                throw new InvalidOperationException("SMTP email is missing. Check your configuration or environment variables.");
            }

            if (string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new InvalidOperationException("SMTP password is missing. Check your configuration or environment variables.");
            }

            // Create and configure the SMTP client
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(smtpEmail, smtpPassword)
            };

            // Create the email message
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpEmail),
                Subject = "VitalMetrics - Confirm your email address",
                Body = emailBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);

            Console.WriteLine($"Email sent successfully to {email}.");
        }
        catch (FileNotFoundException fnfEx)
        {
            Console.WriteLine($"Email template file not found: {fnfEx.Message}");
            throw new InvalidOperationException("Failed to send email due to missing template file.", fnfEx);
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine($"SMTP error occurred: {smtpEx.Message}");
            throw new InvalidOperationException("Failed to send email due to an SMTP issue. Check your email configuration and network.", smtpEx);
        }
        catch (InvalidOperationException invEx)
        {
            Console.WriteLine($"Configuration error: {invEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while sending the email: {ex.Message}");
            throw new InvalidOperationException("Failed to send email due to an unexpected error.", ex);
        }
    }
}
