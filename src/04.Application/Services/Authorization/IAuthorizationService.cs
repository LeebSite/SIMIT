using Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;

namespace Pertamina.SIMIT.Application.Services.Authorization;

public interface IAuthorizationService
{
    Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, CancellationToken cancellationToken);
}
