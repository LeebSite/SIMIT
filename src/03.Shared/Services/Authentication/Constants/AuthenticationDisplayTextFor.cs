using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.Services.Authentication.Constants;

public static class AuthenticationDisplayTextFor
{
    public static readonly string AuthenticationProvider = nameof(AuthenticationProvider).SplitWords();

    public const string Login = nameof(Login);
    public const string Logout = nameof(Logout);

    public const string Account = nameof(Account);
    public static readonly string MySession = nameof(MySession).SplitWords();
    public static readonly string MyClaims = nameof(MyClaims).SplitWords();
    public static readonly string AccessToken = nameof(AccessToken).SplitWords();

    public const string UserId = "User ID";
    public const string Username = nameof(Username);
    public static readonly string AuthenticationTime = nameof(AuthenticationTime).SplitWords();
    public static readonly string DisplayName = nameof(DisplayName).SplitWords();
    public static readonly string CompanyCode = nameof(CompanyCode).SplitWords();
    public const string PositionId = "Position ID";
    public const string EmployeeId = "Employee ID";

    public const string Position = nameof(Position);
    public const string Positions = nameof(Positions);
    public static readonly string SwitchPosition = nameof(SwitchPosition).SplitWords();
}
