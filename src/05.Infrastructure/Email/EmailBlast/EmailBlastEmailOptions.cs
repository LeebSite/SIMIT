namespace Pertamina.SIMIT.Infrastructure.Email.EmailBlast;

public class EmailBlastEmailOptions
{
    public static readonly string SectionKey = $"{nameof(Email)}:{nameof(EmailBlast)}";

    public Guid IdAManClientId { get; set; }
    public Guid IdAManClientSecret { get; set; }
    public string IdAManTokenUrl { get; set; } = default!;
    public string IdAManScopeId { get; set; } = default!;
    public Guid AppId { get; set; }
    public Guid AppSecret { get; set; }
    public string BaseUrl { get; set; } = default!;
    public Endpoints Endpoints { get; set; } = default!;

    public string HealthCheckUrl => $"{BaseUrl}{Endpoints.HealthCheck}";
}

public class Endpoints
{
    public string HealthCheck { get; set; } = default!;
    public string SendEmailWithoutTemplate { get; set; } = default!;
}
