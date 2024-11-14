using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pertamina.SIMIT.Client.Services.BackEnd;
using Pertamina.SIMIT.Client.Services.HealthCheck;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.ApiTests.Client;

public class ClientFixture : IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfigurationRoot _configuration;

    public BackEndOptions BackEndOptions { get; }

    public ClientFixture()
    {
        SetupEnvironmentVariables();

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{AppContext.BaseDirectory}appsettings.{CommonValueFor.EnvironmentName}.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
        BackEndOptions = _configuration.GetSection(BackEndOptions.SectionKey).Get<BackEndOptions>();

        var startup = new Startup(_configuration);
        var services = new ServiceCollection();

        startup.ConfigureServices(services);

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    private static void SetupEnvironmentVariables()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{AppContext.BaseDirectory}appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        Environment.SetEnvironmentVariable(EnvironmentVariables.AspNetCoreEnvironment, configuration["Environment"]);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public HealthCheckService HealthCheckService
    {
        get
        {
            using var scope = _scopeFactory.CreateScope();

            return scope.ServiceProvider.GetRequiredService<HealthCheckService>();
        }
    }
}
