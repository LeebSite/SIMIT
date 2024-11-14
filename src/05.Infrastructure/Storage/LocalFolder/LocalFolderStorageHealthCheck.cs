using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pertamina.SIMIT.Infrastructure.Storage.LocalFolder;

public class LocalFolderStorageHealthCheck : IHealthCheck
{
    private readonly string _path;

    public LocalFolderStorageHealthCheck(string path)
    {
        _path = path;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var testFile = $"{_path}\\HealthCheck_{Guid.NewGuid()}.txt";
            var fileStream = File.Create(testFile);

            fileStream.Close();
            File.Delete(testFile);

            return Task.FromResult(HealthCheckResult.Healthy($"Application has read and write permissions to {_path}"));
        }
        catch (Exception exception)
        {
            return context.Registration.FailureStatus is HealthStatus.Unhealthy
                ? Task.FromResult(HealthCheckResult.Unhealthy(exception.Message))
                : Task.FromResult(HealthCheckResult.Degraded(exception.Message));
        }
    }
}
