using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Ecm;

namespace Pertamina.SIMIT.Infrastructure.Ecm.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneEcmService(this IServiceCollection services)
    {
        services.AddTransient<IEcmService, NoneEcmService>();

        return services;
    }
}
