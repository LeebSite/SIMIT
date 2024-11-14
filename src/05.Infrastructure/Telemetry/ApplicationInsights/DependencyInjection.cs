using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pertamina.SIMIT.Infrastructure.Telemetry.ApplicationInsights;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationInsightsTelemetryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationInsightsTelemetryOptions>(configuration.GetSection(ApplicationInsightsTelemetryOptions.SectionKey));
        services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();

        var applicationInsightsTelemetryOptions = configuration.GetSection(ApplicationInsightsTelemetryOptions.SectionKey).Get<ApplicationInsightsTelemetryOptions>();

        services.AddApplicationInsightsTelemetry(options => options.ConnectionString = applicationInsightsTelemetryOptions.ConnectionString);
        services.AddApplicationInsightsTelemetryProcessor<CustomTelemetryProcessor>();

        return services;
    }
}
