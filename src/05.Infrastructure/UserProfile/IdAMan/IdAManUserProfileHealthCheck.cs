using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pertamina.SIMIT.Shared.Services.HealthCheck.Constants;
using Pertamina.SIMIT.Shared.Services.HealthCheck.Queries.GetHealthCheck;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.UserProfile.IdAMan;

public class IdAManUserProfileHealthCheck : IHealthCheck
{
    private readonly RestClient _restClient;

    public IdAManUserProfileHealthCheck(string healthCheckUrl)
    {
        _restClient = new RestClient(healthCheckUrl);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest(string.Empty, Method.Get);
            var restResponse = await _restClient.ExecuteAsync<GetHealthCheckResponse>(restRequest, cancellationToken);

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
                HealthCheckStatus.Unhealthy => HealthCheckResult.Unhealthy(),
                HealthCheckStatus.Degraded => HealthCheckResult.Degraded(),
                HealthCheckStatus.Healthy => HealthCheckResult.Healthy(),
                HealthCheckStatus.Loading => HealthCheckResult.Unhealthy(),
                _ => HealthCheckResult.Unhealthy(),
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
