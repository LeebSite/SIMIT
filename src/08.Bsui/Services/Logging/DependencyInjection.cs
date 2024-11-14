﻿using Pertamina.SIMIT.Bsui.Services.Logging.None;
using Pertamina.SIMIT.Bsui.Services.Logging.Serilog;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Bsui.Services.Logging;

public static class DependencyInjection
{
    public static IHostBuilder UseLoggingService(this IHostBuilder hostBuilder)
    {
        var configuration = new ConfigurationBuilder()
          .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{CommonValueFor.EnvironmentName}.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();

        var loggingOptions = configuration.GetSection(LoggingOptions.SectionKey).Get<LoggingOptions>();

        switch (loggingOptions.Provider)
        {
            case LoggingProvider.None:
                hostBuilder.UseNoneLoggingService();
                break;
            case LoggingProvider.Serilog:
                hostBuilder.UseSerilogLoggingService();
                break;
            default:
                throw new ArgumentException($"{CommonDisplayTextFor.Unsupported} {nameof(Logging)} {nameof(LoggingOptions.Provider)}: {loggingOptions.Provider}");
        }

        return hostBuilder;
    }
}
