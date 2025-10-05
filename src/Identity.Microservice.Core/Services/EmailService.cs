using System.Threading.Tasks;
using Email.Microservice.Core.Configuration;
using Identity.Microservice.Core.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Identity.Microservice.Core.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _mailSettings;
    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
        
    public async Task SendEmailAsync(string[] recipients, string subject, string body)
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail)
        };
        foreach (var recipient in recipients)
        {
            email.To.Add(MailboxAddress.Parse(recipient));
        }
        email.Subject = subject;
        var builder = new BodyBuilder
        {
            HtmlBody = body
        };
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}