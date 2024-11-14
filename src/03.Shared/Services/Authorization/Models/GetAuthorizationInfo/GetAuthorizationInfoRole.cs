namespace Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;

public class GetAuthorizationInfoRole
{
    public string Name { get; set; } = default!;
    public IList<string> Permissions { get; set; } = new List<string>();
}
