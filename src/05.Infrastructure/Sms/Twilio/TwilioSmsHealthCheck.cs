using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Sms.Twilio;

public class TwilioSmsHealthCheck : IHealthCheck
{
    private readonly RestClient _restClient;

    public TwilioSmsHealthCheck(string healthCheckUrl)
    {
        _restClient = new RestClient(healthCheckUrl);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var restRequest = new RestRequest(string.Empty, Method.Get);
            var restResponse = await _restClient.ExecuteAsync<TwilioSmsHealthCheckResult>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                return HealthCheckResult.Unhealthy(restResponse.ErrorMessage);
            }

            if (restResponse.Data is null)
            {
                throw new Exception($"Failed to deserialize JSON content into {nameof(TwilioSmsHealthCheckResult)}.");
            }

            var name = restResponse.Data.Page.Name;
            var timeZone = restResponse.Data.Page.TimeZone;
            var indicator = restResponse.Data.Status.Indicator;
            var description = restResponse.Data.Status.Description;

            if (indicator == "none")
            {
                return HealthCheckResult.Healthy(description);
            }
            else if (indicator is "maintenance" or "minor")
            {
                var fullDescription = string.IsNullOrWhiteSpace(timeZone)
                    ? description
                    : $"{description} in {timeZone}";

                return HealthCheckResult.Degraded(fullDescription);
            }
            else
            {
                return HealthCheckResult.Unhealthy(description);
            }
        }
        catch (Exception exception)
        {
            return context.Registration.FailureStatus is HealthStatus.Unhealthy
                ? HealthCheckResult.Unhealthy(exception.Message)
                : HealthCheckResult.Degraded(exception.Message);
        }
    }
}

public class TwilioSmsHealthCheckResult
{
    [JsonProperty("page")]
    public Page Page { get; set; } = default!;

    [JsonProperty("status")]
    public Status Status { get; set; } = default!;
}

public class Page
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("url")]
    public string Url { get; set; } = default!;

    [JsonProperty("time_zone")]
    public string TimeZone { get; set; } = default!;

    [JsonProperty("updated_at")]
    public System.DateTime Updated { get; set; }
}

public class Status
{
    [JsonProperty("indicator")]
    public string Indicator { get; set; } = default!;

    [JsonProperty("description")]
    public string Description { get; set; } = default!;
}
