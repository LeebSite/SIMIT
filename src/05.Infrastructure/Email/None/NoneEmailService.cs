using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Services.Email;
using Pertamina.SIMIT.Application.Services.Email.Models.SendEmail;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Email.None;

public class NoneEmailService : IEmailService
{
    private readonly ILogger<NoneEmailService> _logger;

    public NoneEmailService(ILogger<NoneEmailService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Email)} {CommonDisplayTextFor.Service}");
    }

    public Task SendEmailAsync(SendEmailRequest emailModel)
    {
        LogWarning();

        return Task.CompletedTask;
    }
}
