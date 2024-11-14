namespace Pertamina.SIMIT.Infrastructure.Authentication.IdAMan;

public class IdAManAuthenticationOptions
{
    public static readonly string SectionKey = $"{nameof(Authentication)}:{nameof(IdAMan)}";

    public string AuthorityUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;
    public string ObjectId { get; set; } = default!;

    public string HealthCheckUrl => $"{AuthorityUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
}
