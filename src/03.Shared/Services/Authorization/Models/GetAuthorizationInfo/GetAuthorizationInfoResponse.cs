namespace Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;

public class GetAuthorizationInfoResponse
{
    public IList<GetAuthorizationInfoRole> Roles { get; set; } = new List<GetAuthorizationInfoRole>();
    public IList<GetAuthorizationInfoCustomParameter> CustomParameters { get; set; } = new List<GetAuthorizationInfoCustomParameter>();
}
