using Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using Pertamina.SIMIT.Shared.Services.Authorization.Models.GetPositions;

namespace Pertamina.SIMIT.Bsui.Services.Authorization;

public interface IAuthorizationService
{
    Task<GetPositionsResponse> GetPositionsAsync(string username, string accessToken, CancellationToken cancellationToken = default);
    Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, string accessToken, CancellationToken cancellationToken = default);
}
