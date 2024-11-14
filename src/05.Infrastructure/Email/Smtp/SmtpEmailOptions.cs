namespace Pertamina.SIMIT.Infrastructure.Email.Smtp;

public class SmtpEmailOptions
{
    public static readonly string SectionKey = $"{nameof(Email)}:{nameof(Smtp)}";

    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool EnableSsl { get; set; }
}
