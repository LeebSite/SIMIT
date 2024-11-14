using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Infrastructure.AppInfo;
using Pertamina.SIMIT.Infrastructure.HealthCheck.Storages.MySql;
using Pertamina.SIMIT.Infrastructure.HealthCheck.Storages.SqlServer;
using Pertamina.SIMIT.Infrastructure.Logging;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Infrastructure.HealthCheck;

public static class DependencyInjection
{
    public static IHealthChecksBuilder AddHealthCheckService(this IServiceCollection services, IConfiguration configuration)
    {
        var appInfoOptions = configuration.GetSection(AppInfoOptions.SectionKey).Get<AppInfoOptions>();
        var healthCheckOptions = configuration.GetSection(HealthCheckOptions.SectionKey).Get<HealthCheckOptions>();

        if (healthCheckOptions.UI.Enabled)
        {
            var healthChecksUIBuilder = services.AddHealthChecksUI(settings => settings.AddHealthCheckEndpoint($"{appInfoOptions.FullName} {nameof(Infrastructure)}", $"{healthCheckOptions.UI.AbsoluteUri}{healthCheckOptions.Endpoint}"));

            switch (healthCheckOptions.UI.Storage.Provider)
            {
                case HealthCheckStorageProvider.SqlServer:
                    var sqlServerOptions = configuration.GetSection(SqlServerOptions.SectionKey).Get<SqlServerOptions>();
                    healthChecksUIBuilder.AddSqlServerStorage(sqlServerOptions);
                    break;
                case HealthCheckStorageProvider.MySql:
                    var mySqlOptions = configuration.GetSection(MySqlOptions.SectionKey).Get<MySqlOptions>();
                    healthChecksUIBuilder.AddMySqlStorage(mySqlOptions);
                    break;
                default:
                    throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(HealthCheck)} {nameof(HealthCheckOptions.UI)} {nameof(HealthCheckOptions.UI.Storage)} {nameof(HealthCheckOptions.UI.Storage.Provider)}: {healthCheckOptions.UI.Storage.Provider}");
            }
        }
        else
        {
            LoggingHelper
                .CreateLogger()
                .LogWarning("{ServiceName} is not enabled.", $"{nameof(HealthCheck)} {nameof(HealthCheckOptions.UI)}");
        }

        return services.AddHealthChecks();
    }

    public static IApplicationBuilder UseHealthCheckService(this IApplicationBuilder app, IConfiguration configuration)
    {
        var healthCheckOptions = configuration.GetSection(HealthCheckOptions.SectionKey).Get<HealthCheckOptions>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(healthCheckOptions.Endpoint, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            if (healthCheckOptions.UI.Enabled)
            {
                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = healthCheckOptions.UI.Endpoints.UI;
                    options.ApiPath = healthCheckOptions.UI.Endpoints.Api;
                    options.AddCustomStylesheet(@"wwwroot\healthchecks\site.css");
                });
            }
        });

        return app;
    }
}
