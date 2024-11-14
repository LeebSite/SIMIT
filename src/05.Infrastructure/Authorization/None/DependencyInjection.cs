using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Authorization;

namespace Pertamina.SIMIT.Infrastructure.Authorization.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneAuthorizationService(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationService, NoneAuthorizationService>();

        return services;
    }
}
