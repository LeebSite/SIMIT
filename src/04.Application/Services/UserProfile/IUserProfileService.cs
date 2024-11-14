using Pertamina.SIMIT.Application.Services.UserProfile.Models.GetUserProfile;

namespace Pertamina.SIMIT.Application.Services.UserProfile;

public interface IUserProfileService
{
    Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken);
}
