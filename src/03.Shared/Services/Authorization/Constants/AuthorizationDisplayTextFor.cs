using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Services.Authorization.Constants;

public static class AuthorizationDisplayTextFor
{
    public static readonly string AuthorizationProvider = nameof(AuthorizationProvider).SplitWords();
    public const string Positions = nameof(Positions);
    public static readonly string AuthorizationInfo = nameof(AuthorizationInfo).SplitWords();

    public const string Permission = nameof(Permission);
    public const string Permissions = nameof(Permissions);
    public static readonly string CustomParameters = nameof(CustomParameters).SplitWords();
    public const string Key = nameof(Key);
    public const string Value = nameof(Value);
}
