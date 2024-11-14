namespace Pertamina.SIMIT.Bsui.Services.Authentication;

public class AuthenticationOptions
{
    public const string SectionKey = nameof(Authentication);

    public int RefreshRateInSeconds { get; set; }
    public string Provider { get; set; } = AuthenticationProvider.None;
}

public static class AuthenticationProvider
{
    public const string None = nameof(None);
    public const string IdAMan = nameof(IdAMan);
    public const string IS4IM = nameof(IS4IM);
}
