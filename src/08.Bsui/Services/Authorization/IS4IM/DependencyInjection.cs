using Pertamina.SIMIT.Shared.Services.Authorization.Constants;

namespace Pertamina.SIMIT.Bsui.Services.Authorization.IS4IM;

public static class DependencyInjection
{
    public static IServiceCollection AddIS4IMAuthorizationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IS4IMAuthorizationOptions>(configuration.GetSection(IS4IMAuthorizationOptions.SectionKey));
        services.AddTransient<IAuthorizationService, IS4IMAuthorizationService>();

        services.AddAuthorization(config =>
        {
            foreach (var permission in Permissions.All)
            {
                config.AddPolicy(permission, policy => policy.RequireClaim(AuthorizationClaimTypes.Permission, permission));
            }
        });

        return services;
    }
}
