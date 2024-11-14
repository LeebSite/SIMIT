using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.BackgroundJob;
using Pertamina.SIMIT.Application.Services.Sms;
using Pertamina.SIMIT.Application.Services.Sms.Models.SendSms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

using PhoneNumber = Twilio.Types.PhoneNumber;

namespace Pertamina.SIMIT.Infrastructure.Sms.Twilio;

public class TwilioSmsService : ISmsService
{
    private readonly TwilioSmsOptions _twilioSmsOptions;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly ILogger<TwilioSmsService> _logger;

    public TwilioSmsService(
        IOptions<TwilioSmsOptions> twilioSmsOptions,
        IBackgroundJobService backgroundJobService,
        ILogger<TwilioSmsService> logger)
    {
        _twilioSmsOptions = twilioSmsOptions.Value;
        _backgroundJobService = backgroundJobService;
        _logger = logger;
    }

    public async Task SendSmsAsync(SendSmsRequest smsModel)
    {
        await _backgroundJobService.RunJob(() => SendAsync(smsModel));
    }

    public async Task SendAsync(SendSmsRequest smsModel)
    {
        try
        {
            TwilioClient.Init(_twilioSmsOptions.AccountId, _twilioSmsOptions.AuthenticationToken);

            var messageResource = await MessageResource.CreateAsync(
                body: smsModel.Message,
                from: new PhoneNumber(_twilioSmsOptions.FromPhoneNumber),
                to: new PhoneNumber(smsModel.To)
            );

            _logger.LogInformation("Executing method {MethodName} with Message Resource {@MessageResource}", nameof(SendAsync), messageResource);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with model {@Model}", nameof(SendAsync), smsModel);

            throw;
        }
    }
}
