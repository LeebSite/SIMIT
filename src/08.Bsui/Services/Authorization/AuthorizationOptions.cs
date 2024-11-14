namespace Pertamina.SIMIT.Bsui.Services.Authorization;

public class AuthorizationOptions
{
    public const string SectionKey = nameof(Authorization);

    public string Provider { get; set; } = AuthorizationProvider.None;
}

public static class AuthorizationProvider
{
    public const string None = nameof(None);
    public const string IdAMan = nameof(IdAMan);
    public const string IS4IM = nameof(IS4IM);
}
