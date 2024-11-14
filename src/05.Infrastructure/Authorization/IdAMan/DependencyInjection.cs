using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.Authorization;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.Authorization.IdAMan;

public static class DependencyInjection
{
    public static IServiceCollection AddIdAManAuthorizationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IdAManAuthorizationOptions>(configuration.GetSection(IdAManAuthorizationOptions.SectionKey));
        services.AddTransient<IAuthorizationService, IdAManAuthorizationService>();

        var idAManAuthorizationOptions = configuration.GetSection(IdAManAuthorizationOptions.SectionKey).Get<IdAManAuthorizationOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Authorization)} {CommonDisplayTextFor.Service} ({nameof(IdAMan)})",
            instance: new IdAManAuthorizationHealthCheck(idAManAuthorizationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
