using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Pertamina.SIMIT.Infrastructure.Telemetry.ApplicationInsights;

public class CustomTelemetryProcessor : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }

    public CustomTelemetryProcessor(ITelemetryProcessor next)
    {
        Next = next;
    }

    public void Process(ITelemetry telemetry)
    {
        if (telemetry is RequestTelemetry requestTelemetry)
        {
            if (requestTelemetry.Name == "GET /healthcheck")
            {
                return;
            }

            if (requestTelemetry.Name == "GET /hc-api")
            {
                return;
            }
        }

        Next.Process(telemetry);
    }
}
