using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.BackgroundJob;
using Pertamina.SIMIT.Application.Services.Email;
using Pertamina.SIMIT.Application.Services.Email.Models.SendEmail;

namespace Pertamina.SIMIT.Infrastructure.Email;

public class EmailService : IEmailService
{
    private const char Comma = ',';

    private readonly IBackgroundJobService _backgroundJobService;
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailService> _logger;
    private readonly string _emailProvider;
    private readonly string _frontEndBaseUrl;

    public EmailService(
        IBackgroundJobService backgroundJobService,
        IFluentEmail fluentEmail,
        ILogger<EmailService> logger,
        IOptions<EmailOptions> emailOptions)
    {
        _backgroundJobService = backgroundJobService;
        _fluentEmail = fluentEmail;
        _logger = logger;
        _emailProvider = emailOptions.Value.Provider;
        _frontEndBaseUrl = emailOptions.Value.FrontEndBaseUrl;
    }

    public async Task SendEmailAsync(SendEmailRequest emailModel)
    {
        await _backgroundJobService.RunJob(() => SendAsync(emailModel));
    }

    public async Task SendAsync(SendEmailRequest emailModel)
    {
        var toAddresses = string.Join(Comma, emailModel.Tos);

        _logger.LogInformation("Attempting to send email to {ToAddresses} with subject [{Subject}] using provider {EmailProvider}.", toAddresses, emailModel.Subject, _emailProvider);

        var body = emailModel.Body.Replace("{{FrontEndBaseUrl}}", _frontEndBaseUrl);

        var response = await _fluentEmail
              .To(emailModel.Tos.Select(x => new Address(x)))
              .CC(emailModel.Ccs.Select(x => new Address(x)))
              .BCC(emailModel.Bccs.Select(x => new Address(x)))
              .Subject(emailModel.Subject)
              .Body(body, true)
              .SendAsync();

        if (!response.Successful)
        {
            var exception = new Exception(string.Join(Environment.NewLine, response.ErrorMessages));

            _logger.LogError(exception, "Error in sending email to {ToAddress} with subject [{Subject}] using provider {EmailProvider}", toAddresses, emailModel.Subject, _emailProvider);

            throw exception;
        }
    }
}
