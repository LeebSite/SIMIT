using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Storage;

namespace Pertamina.SIMIT.Infrastructure.Storage.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneStorageService(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, NoneStorageService>();

        return services;
    }
}
