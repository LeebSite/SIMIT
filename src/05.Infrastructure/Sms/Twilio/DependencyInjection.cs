using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.Sms;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Sms.Twilio;

public static class DependencyInjection
{
    public static IServiceCollection AddTwilioSmsService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<TwilioSmsOptions>(configuration.GetSection(TwilioSmsOptions.SectionKey));
        services.AddSingleton<ISmsService, TwilioSmsService>();

        var twilioSmsOptions = configuration.GetSection(TwilioSmsOptions.SectionKey).Get<TwilioSmsOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Sms).ToUpper()} {CommonDisplayTextFor.Service} ({nameof(Twilio)})",
            instance: new TwilioSmsHealthCheck(twilioSmsOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Degraded,
            tags: default));

        return services;
    }
}
