using Pertamina.SIMIT.Bsui.Services.Logging;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Bsui.Services.Authentication.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneAuthenticationService(this IServiceCollection services)
    {
        LoggingHelper
            .CreateLogger()
            .LogWarning("{ServiceName} is set to None.", $"{nameof(Authentication)} {CommonDisplayTextFor.Service}");

        return services;
    }
}
