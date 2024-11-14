using Pertamina.SIMIT.Bsui.Services.Geolocation.Darnton;
using Pertamina.SIMIT.Bsui.Services.Geolocation.None;

namespace Pertamina.SIMIT.Bsui.Services.Geolocation;

public static class DependencyInjection
{
    public static IServiceCollection AddGeolocationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeolocationOptions>(configuration.GetSection(GeolocationOptions.SectionKey));

        var geolocationOptions = configuration.GetSection(GeolocationOptions.SectionKey).Get<GeolocationOptions>();

        if (geolocationOptions.Enabled)
        {
            services.AddDarntonGeolocationService();
        }
        else
        {
            services.AddNoneGeolocationService();
        }

        return services;
    }
}
