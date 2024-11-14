namespace Pertamina.SIMIT.Infrastructure.Email;

public class EmailOptions
{
    public const string SectionKey = nameof(Email);

    public string Provider { get; set; } = EmailProvider.None;
    public string SenderDisplayName { get; set; } = default!;
    public string SenderEmailAddress { get; set; } = default!;
    public string FrontEndBaseUrl { get; set; } = default!;
}

public static class EmailProvider
{
    public const string None = nameof(None);
    public const string Smtp = nameof(Smtp);
    public const string EmailBlast = nameof(EmailBlast);
}
