using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Email.EmailBlast;

public class EmailBlastEmailHealthCheck : IHealthCheck
{
    private readonly RestClient _restClient;

    public EmailBlastEmailHealthCheck(string healthCheckUrl)
    {
        _restClient = new RestClient(healthCheckUrl);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest(string.Empty, Method.Get);
            var restResponse = await _restClient.ExecuteAsync<EmailBlastHealthCheckResult>(restRequest, cancellationToken);

            if (restResponse.StatusCode == 0)
            {
                return HealthCheckResult.Degraded(restResponse.ErrorMessage);
            }

            if (!restResponse.IsSuccessful)
            {
                return HealthCheckResult.Unhealthy(restResponse.ErrorMessage);
            }

            if (restResponse.Data is null)
            {
                return HealthCheckResult.Unhealthy();
            }

            return restResponse.Data.Status switch
            {
                EmailBlastHealthCheckResult.Healthy => HealthCheckResult.Healthy(),
                EmailBlastHealthCheckResult.Degraded => HealthCheckResult.Degraded(),
                _ => HealthCheckResult.Unhealthy()
            };
        }
        catch (Exception exception)
        {
            return context.Registration.FailureStatus is HealthStatus.Unhealthy
                ? HealthCheckResult.Unhealthy(exception.Message)
                : HealthCheckResult.Degraded(exception.Message);
        }
    }
}

public class EmailBlastHealthCheckResult
{
    public const string Healthy = nameof(Healthy);
    public const string Degraded = nameof(Degraded);

    public string Status { get; set; } = default!;
}
