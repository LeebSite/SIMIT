namespace Pertamina.SIMIT.Infrastructure.UserProfile;

public class UserProfileOptions
{
    public const string SectionKey = nameof(UserProfile);

    public string Provider { get; set; } = UserProfileProvider.None;
}

public static class UserProfileProvider
{
    public const string None = nameof(None);
    public const string IdAMan = nameof(IdAMan);
    public const string IS4IM = nameof(IS4IM);
}
