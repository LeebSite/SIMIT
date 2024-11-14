using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.Sms;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Sms.Jatis;

public static class DependencyInjection
{
    public static IServiceCollection AddJatisSmsService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<JatisSmsOptions>(configuration.GetSection(JatisSmsOptions.SectionKey));
        services.AddSingleton<ISmsService, JatisSmsService>();

        var jatisSmsOptions = configuration.GetSection(JatisSmsOptions.SectionKey).Get<JatisSmsOptions>();

        healthChecksBuilder.AddUrlGroup(
            new Uri(jatisSmsOptions.Url),
            name: $"{nameof(Sms).ToUpper()} {CommonDisplayTextFor.Service} ({nameof(Jatis)})",
            failureStatus: HealthStatus.Degraded);

        return services;
    }
}
