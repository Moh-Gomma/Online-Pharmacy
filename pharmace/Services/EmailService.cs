using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpSettings = _configuration.GetSection("EmailSettings");

        string host = smtpSettings["SmtpServer"];
        string portString = smtpSettings["SmtpPort"];
        string username = smtpSettings["SmtpUsername"];
        string password = smtpSettings["SmtpPassword"];
        string enableSslString = smtpSettings["EnableSsl"];

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(portString) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(enableSslString))
        {
            throw new InvalidOperationException("SMTP settings are not configured properly.");
        }

        if (!int.TryParse(portString, out int port))
        {
            throw new InvalidOperationException("SMTP port is not a valid number.");
        }

        if (!bool.TryParse(enableSslString, out bool enableSsl))
        {
            throw new InvalidOperationException("EnableSsl setting is not a valid boolean.");
        }

        var client = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = enableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(username),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
