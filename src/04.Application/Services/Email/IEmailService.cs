using Pertamina.SIMIT.Application.Services.Email.Models.SendEmail;

namespace Pertamina.SIMIT.Application.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest sendEmailRequest);
}
