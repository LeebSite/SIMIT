namespace Pertamina.SIMIT.Infrastructure.Ecm.Idms;

public class IdmsEcmOptions
{
    public static readonly string SectionKey = $"{nameof(Ecm)}:{nameof(Idms)}";

    public string TokenUrl { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string BaseUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;
    public string ApplicationId { get; set; } = default!;
    public string ScopeId { get; set; } = default!;

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string Documents { get; set; } = default!;
    public string HealthCheck { get; set; } = default!;
}
