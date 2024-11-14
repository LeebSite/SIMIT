using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.BackgroundJob;

namespace Pertamina.SIMIT.Infrastructure.BackgroundJob.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneBackgroundJobService(this IServiceCollection services)
    {
        services.AddTransient<IBackgroundJobService, NoneBackgroundJobService>();

        return services;
    }
}
