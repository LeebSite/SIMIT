using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.Authorization;

namespace Pertamina.SIMIT.Infrastructure.Authorization.IS4IM;

public static class DependencyInjection
{
    public static IServiceCollection AddIS4IMAuthorizationService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IS4IMAuthorizationOptions>(configuration.GetSection(IS4IMAuthorizationOptions.SectionKey));
        services.AddTransient<IAuthorizationService, IS4IMAuthorizationService>();

        var is4imAuthorizationOptions = configuration.GetSection(IS4IMAuthorizationOptions.SectionKey).Get<IS4IMAuthorizationOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(Authorization)} Service ({nameof(IS4IM)})",
            instance: new IS4IMAuthorizationHealthCheck(is4imAuthorizationOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
