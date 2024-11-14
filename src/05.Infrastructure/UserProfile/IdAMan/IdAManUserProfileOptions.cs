namespace Pertamina.SIMIT.Infrastructure.UserProfile.IdAMan;

public class IdAManUserProfileOptions
{
    public static readonly string SectionKey = $"{nameof(UserProfile)}:{nameof(IdAMan)}";

    public string BaseUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;
    public string TokenUrl { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public IList<string> Scopes { get; set; } = new List<string>();

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string Users { get; set; } = default!;
    public string HealthCheck { get; set; } = default!;
}
