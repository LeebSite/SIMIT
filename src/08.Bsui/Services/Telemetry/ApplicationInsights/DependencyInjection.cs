﻿using Microsoft.ApplicationInsights.Extensibility;

namespace Pertamina.SIMIT.Bsui.Services.Telemetry.ApplicationInsights;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationInsightsTelemetryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationInsightsTelemetryOptions>(configuration.GetSection(ApplicationInsightsTelemetryOptions.SectionKey));

        var applicationInsightsTelemetryOptions = configuration.GetSection(ApplicationInsightsTelemetryOptions.SectionKey).Get<ApplicationInsightsTelemetryOptions>();

        services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
        services.AddApplicationInsightsTelemetry(options => options.ConnectionString = applicationInsightsTelemetryOptions.ConnectionString);
        services.AddApplicationInsightsTelemetryProcessor<CustomTelemetryProcessor>();

        return services;
    }
}
