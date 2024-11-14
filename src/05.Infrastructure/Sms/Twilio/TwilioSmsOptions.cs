namespace Pertamina.SIMIT.Infrastructure.Sms.Twilio;

public class TwilioSmsOptions
{
    public static readonly string SectionKey = $"{nameof(Sms)}:{nameof(Twilio)}";

    public string AccountId { get; set; } = default!;
    public string AuthenticationToken { get; set; } = default!;
    public string FromPhoneNumber { get; set; } = default!;
    public string HealthCheckUrl { get; set; } = default!;
}
