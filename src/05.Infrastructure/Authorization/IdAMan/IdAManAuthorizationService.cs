using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.Authorization;
using Pertamina.SIMIT.Infrastructure.Authorization.IdAMan.Models.GetAuhtorizationInfo;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Services.Authorization.Constants;
using Pertamina.SIMIT.Shared.Services.Authorization.Models.GetAuthorizationInfo;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Authorization.IdAMan;

public class IdAManAuthorizationService : IAuthorizationService
{
    private readonly IdAManAuthorizationOptions _idAManAuthorizationOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RestClient _restClient;

    public IdAManAuthorizationService(IOptions<IdAManAuthorizationOptions> idAManAuthorizationOptions, IHttpContextAccessor httpContextAccessor)
    {
        _idAManAuthorizationOptions = idAManAuthorizationOptions.Value;
        _httpContextAccessor = httpContextAccessor;
        _restClient = new RestClient(_idAManAuthorizationOptions.BaseUrl);
    }

    public async Task<GetAuthorizationInfoResponse> GetAuthorizationInfoAsync(string positionId, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new Exception("HttpContext is null");
        }

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OidcConstants.TokenResponse.AccessToken);
        var restRequest = new RestRequest($"{_idAManAuthorizationOptions.Endpoints.AuthorizationInfo}/{positionId}?applicationId={_idAManAuthorizationOptions.ObjectId}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        try
        {
            var restResponse = await _restClient.ExecuteAsync<GetAuhtorizationInfoIdAMan[]>(restRequest, cancellationToken);

            if (!restResponse.IsSuccessful)
            {
                if (restResponse.ErrorException is not null)
                {
                    throw restResponse.ErrorException;
                }

                throw new Exception($"Failed to retreive {AuthorizationDisplayTextFor.AuthorizationInfo} from {_restClient.BuildUri(restRequest)}");
            }

            if (restResponse.Data is null)
            {
                throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(GetAuhtorizationInfoIdAMan)}.");
            }

            var authorizationInfo = new GetAuthorizationInfoResponse();

            if (restResponse.Data.Any())
            {
                var idAManApplication = restResponse.Data.First();

                foreach (var role in idAManApplication.Roles)
                {
                    var authorizationInfoRole = new GetAuthorizationInfoRole
                    {
                        Name = role.Name
                    };

                    if (role.Permissions is not null)
                    {
                        foreach (var permission in role.Permissions)
                        {
                            authorizationInfoRole.Permissions.Add(permission);
                        }
                    }

                    authorizationInfo.Roles.Add(authorizationInfoRole);
                }

                foreach (var customParameter in idAManApplication.CustomParameters.SelectMany(x => x).ToList())
                {
                    authorizationInfo.CustomParameters.Add(new GetAuthorizationInfoCustomParameter
                    {
                        Key = customParameter.Key,
                        Value = customParameter.Value
                    });
                }
            }

            return authorizationInfo;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
