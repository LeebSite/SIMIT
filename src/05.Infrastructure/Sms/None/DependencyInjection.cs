using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Sms;

namespace Pertamina.SIMIT.Infrastructure.Sms.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneSmsService(this IServiceCollection services)
    {
        services.AddSingleton<ISmsService, NoneSmsService>();

        return services;
    }
}
