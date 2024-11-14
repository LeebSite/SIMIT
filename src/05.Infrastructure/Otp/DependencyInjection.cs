using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Otp;

namespace Pertamina.SIMIT.Infrastructure.Otp;

public static class DependencyInjection
{
    public static IServiceCollection AddOtpService(this IServiceCollection services)
    {
        services.AddTransient<IOtpService, OtpService>();

        return services;
    }
}
