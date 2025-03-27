using System.Net.Mail;
using System.Net;
using EmployeeManagementSystem.IServices;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_config["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Email"],
                _config["EmailSettings:Password"]),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:Email"]),
            Subject = subject,
            Body = body
        };

        mailMessage.To.Add(to);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw;
        }
        Console.WriteLine($"Email sent to {to}: {subject} - {body}");
    }
}
