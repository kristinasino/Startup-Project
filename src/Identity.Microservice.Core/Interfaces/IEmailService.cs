using System.Threading.Tasks;

namespace Identity.Microservice.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string[] recipients, string subject, string body);
}