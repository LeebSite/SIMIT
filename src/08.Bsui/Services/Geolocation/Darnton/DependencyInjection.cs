﻿using Darnton.Blazor.DeviceInterop.Geolocation;

namespace Pertamina.SIMIT.Bsui.Services.Geolocation.Darnton;

public static class DependencyInjection
{
    public static IServiceCollection AddDarntonGeolocationService(this IServiceCollection services)
    {
        services.AddScoped<IGeolocationService, GeolocationService>();

        return services;
    }
}
