using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.DomainEvent;

namespace Pertamina.SIMIT.Infrastructure.DomainEvent;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainEventService(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventService, DomainEventService>();

        return services;
    }
}
