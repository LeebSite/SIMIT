using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Application.Services.UserProfile;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Infrastructure.UserProfile.IdAMan;

public static class DependencyInjection
{
    public static IServiceCollection AddIdAManUserProfileService(this IServiceCollection services, IConfiguration configuration, IHealthChecksBuilder healthChecksBuilder)
    {
        services.Configure<IdAManUserProfileOptions>(configuration.GetSection(IdAManUserProfileOptions.SectionKey));
        services.AddTransient<IUserProfileService, IdAManUserProfileService>();

        var idAManUserProfileOptions = configuration.GetSection(IdAManUserProfileOptions.SectionKey).Get<IdAManUserProfileOptions>();

        healthChecksBuilder.Add(new HealthCheckRegistration(
            name: $"{nameof(UserProfile).SplitWords()} {CommonDisplayTextFor.Service} ({nameof(IdAMan)})",
            instance: new IdAManUserProfileHealthCheck(idAManUserProfileOptions.HealthCheckUrl),
            failureStatus: HealthStatus.Unhealthy,
            tags: default));

        return services;
    }
}
