using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.UserProfile;
using Pertamina.SIMIT.Application.Services.UserProfile.Models.GetUserProfile;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.UserProfile.IS4IM;

public class IS4IMUserProfileService : IUserProfileService
{
    private readonly IS4IMUserProfileOptions _is4imUserProfileOptions;
    private readonly RestClient _restClient;

    public IS4IMUserProfileService(IOptions<IS4IMUserProfileOptions> is4imUserProfileOptions)
    {
        _is4imUserProfileOptions = is4imUserProfileOptions.Value;
        _restClient = new RestClient(_is4imUserProfileOptions.BaseUrl);
    }

    public async Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"{_is4imUserProfileOptions.Endpoints.Users}/{username}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync<GetUserProfileResponse>(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
        {
            throw new Exception($"Failed to get User Profile for {username} from {_restClient.BuildUri(restRequest)}");
        }

        if (restResponse.Data is null)
        {
            throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(GetUserProfileResponse)}.");
        }

        return restResponse.Data;
    }
}
