using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Services.Sms;
using Pertamina.SIMIT.Application.Services.Sms.Models.SendSms;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Sms.None;

public class NoneSmsService : ISmsService
{
    private readonly ILogger<NoneSmsService> _logger;

    public NoneSmsService(ILogger<NoneSmsService> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Sms).ToUpper()} {CommonDisplayTextFor.Service}");
    }

    public Task SendSmsAsync(SendSmsRequest smsModel)
    {
        LogWarning();

        return Task.CompletedTask;
    }
}
