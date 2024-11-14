using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Persistence;

namespace Pertamina.SIMIT.Infrastructure.Persistence.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNonePersistenceService(this IServiceCollection services)
    {
        services.AddScoped<ISIMITDbContext, NoneSIMITDbContext>();

        return services;
    }
}
