namespace Pertamina.SIMIT.Infrastructure.Sms.Jatis;

public class JatisSmsOptions
{
    public static readonly string SectionKey = $"{nameof(Sms)}:{nameof(Jatis)}";

    public string Url { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Division { get; set; } = default!;
    public string UploadBy { get; set; } = default!;
}
