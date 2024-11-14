using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.CurrentUser;

namespace Pertamina.SIMIT.Infrastructure.CurrentUser;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
