namespace Pertamina.SIMIT.Infrastructure.Authentication.IS4IM;

public class IS4IMAuthenticationOptions
{
    public static readonly string SectionKey = $"{nameof(Authentication)}:{nameof(IS4IM)}";

    public string AuthorityUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;
    public string ObjectId { get; set; } = default!;

    public string HealthCheckUrl => $"{AuthorityUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
}
