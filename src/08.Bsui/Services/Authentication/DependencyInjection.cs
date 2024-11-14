using Microsoft.AspNetCore.Components.Authorization;
using Pertamina.SIMIT.Bsui.Services.Authentication.IdAMan;
using Pertamina.SIMIT.Bsui.Services.Authentication.IS4IM;
using Pertamina.SIMIT.Bsui.Services.Authentication.None;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Bsui.Services.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionKey));
        services.AddScoped<AuthenticationStateProvider, AuthorizedAuthenticationStateProvider>();

        var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionKey).Get<AuthenticationOptions>();

        switch (authenticationOptions.Provider)
        {
            case AuthenticationProvider.None:
                services.AddNoneAuthenticationService();
                break;
            case AuthenticationProvider.IdAMan:
                services.AddIdAManAuthentication(configuration);
                break;
            case AuthenticationProvider.IS4IM:
                services.AddIS4IMAuthentication(configuration);
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Authentication)} {nameof(AuthenticationOptions.Provider)}: {authenticationOptions.Provider}");
        }

        return services;
    }
}
