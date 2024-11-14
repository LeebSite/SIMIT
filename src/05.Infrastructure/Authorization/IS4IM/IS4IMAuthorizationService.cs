using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.Authorization;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Authorization.IS4IM;

public class IS4IMAuthorizationService : IAuthorizationService
{
    private readonly IS4IMAuthorizationOptions _is4imAuthorizationOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RestClient _restClient;

    public IS4IMAuthorizationService(IOptions<IS4IMAuthorizationOptions> is4imAuthorizationOptions, IHttpContextAccessor httpContextAccessor)
    {
        _is4imAuthorizationOptions = is4imAuthorizationOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _restClient = new RestClient(_is4imAuthorizationOptions.BaseUrl);
    }

    public async Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new Exception("HttpContext is null");
        }

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OidcConstants.TokenResponse.AccessToken);
        var restRequest = new RestRequest($"{_is4imAuthorizationOptions.Endpoints.AuthorizationInfo}/{positionId}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        try
        {
            var restResponse = await _restClient.ExecuteAsync<GetAuthorizationInfoResponse>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.ErrorException is null)
                {
                    throw new Exception($"Failed to retreive Authorization Info from {_restClient.BuildUri(restRequest)}");
                }

                throw restResponse.ErrorException;
            }

            if (restResponse.Data is null)
            {
                throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(GetAuthorizationInfoResponse)}.");
            }

            return restResponse.Data;
        }
        catch (Exception)
        {
            throw;
        }
    }

}
