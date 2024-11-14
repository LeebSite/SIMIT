using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Application.Services.UserProfile;

namespace Pertamina.SIMIT.Infrastructure.UserProfile.None;

public static class DependencyInjection
{
    public static IServiceCollection AddNoneUserProfileService(this IServiceCollection services)
    {
        services.AddTransient<IUserProfileService, NoneUserProfileService>();

        return services;
    }
}
