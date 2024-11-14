using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.DateAndTime;

namespace Pertamina.SIMIT.Infrastructure.DateAndTime;

public static class DependencyInjection
{
    public static IServiceCollection AddDateAndTimeService(this IServiceCollection services)
    {
        services.AddTransient<IDateAndTimeService, DateAndTimeService>();

        return services;
    }
}
