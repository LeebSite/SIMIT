namespace Pertamina.SIMIT.Infrastructure.UserProfile.IS4IM;

public class IS4IMUserProfileOptions
{
    public static readonly string SectionKey = $"{nameof(UserProfile)}:{nameof(IS4IM)}";

    public string BaseUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string Users { get; set; } = default!;
    public string HealthCheck { get; set; } = default!;
}
