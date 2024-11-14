using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.Email;

namespace Pertamina.SIMIT.Infrastructure.Email.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneEmailService(this IServiceCollection services)
    {
        services.AddSingleton<IEmailService, NoneEmailService>();

        return services;
    }
}
