using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Infrastructure.Sms.Jatis;
using Pertamina.SIMIT.Infrastructure.Sms.None;
using Pertamina.SIMIT.Infrastructure.Sms.Twilio;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Sms;

public static class DependencyInjection
{
    public static IServiceCollection AddSmsService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        var smsOptions = configuration.GetSection(SmsOptions.SectionKey).Get<SmsOptions>();

        switch (smsOptions.Provider)
        {
            case SmsProvider.None:
                services.AddNoneSmsService();
                break;
            case SmsProvider.Twilio:
                services.AddTwilioSmsService(configuration, healthChecksBuilder);
                break;
            case SmsProvider.Jatis:
                services.AddJatisSmsService(configuration, healthChecksBuilder);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Sms).ToUpper()} {nameof(SmsOptions.Provider)}: {smsOptions.Provider}");
        }

        return services;
    }
}
