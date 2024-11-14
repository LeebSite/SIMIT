using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Client.Services.BackEnd;
using Pertamina.SIMIT.Client.Services.HealthCheck;
using Pertamina.SIMIT.Client.Services.UserInfo;

namespace Pertamina.SIMIT.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthCheckService();
        services.AddBackEndService(configuration);
        services.AddUserInfoService();

        return services;
    }
}
