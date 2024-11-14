namespace Pertamina.SIMIT.Infrastructure.Sms;

public class SmsOptions
{
    public const string SectionKey = nameof(Sms);

    public string Provider { get; set; } = SmsProvider.None;
}

public static class SmsProvider
{
    public const string None = nameof(None);
    public const string Twilio = nameof(Twilio);
    public const string Jatis = nameof(Jatis);
}
