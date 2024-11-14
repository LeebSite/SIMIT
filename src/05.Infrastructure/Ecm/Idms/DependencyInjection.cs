using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.Ecm;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Ecm.Idms;

public static class DependencyInjection
{
    public static IServiceCollection AddIdmsEcmService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IdmsEcmOptions>(configuration.GetSection(IdmsEcmOptions.SectionKey));
        services.AddTransient<IEcmService, IdmsEcmService>();

        var idmsEcmOptions = configuration.GetSection(IdmsEcmOptions.SectionKey).Get<IdmsEcmOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Ecm).ToUpper()} {CommonDisplayTextFor.Service} ({nameof(Idms)})",
            instance: new IdmsEcmHealthCheck(idmsEcmOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Degraded,
            tags: default));

        return services;
    }
}
