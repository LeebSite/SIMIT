using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Pertamina.SIMIT.Application.Services.UserProfile;
using Pertamina.SIMIT.Application.Services.UserProfile.Models.GetUserProfile;
using Pertamina.SIMIT.Shared.Common.Constants;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.UserProfile.IdAMan;

public class IdAManUserProfileService : IUserProfileService
{
    private readonly IdAManUserProfileOptions _idAManUserProfileOptions;
    private readonly RestClient _restClient;

    public IdAManUserProfileService(IOptions<IdAManUserProfileOptions> idAManUserProfileOptions)
    {
        _idAManUserProfileOptions = idAManUserProfileOptions.Value;
        _restClient = new RestClient(_idAManUserProfileOptions.BaseUrl);
    }

    public async Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        var userProfile = new GetUserProfileResponse();
        var accessToken = await GetAccessToken(cancellationToken);
        var restRequest = new RestRequest($"{_idAManUserProfileOptions.Endpoints.Users}/{username}", Method.Get);

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");

        var restResponse = await _restClient.ExecuteAsync(restRequest, cancellationToken);

        if (restResponse.IsSuccessful && !string.IsNullOrWhiteSpace(restResponse.Content))
        {
            var jObjectUser = JObject.Parse(restResponse.Content);

            userProfile.EmailAddress = username;

            var jTokenId = jObjectUser["id"];

            if (jTokenId is not null)
            {
                var jTokenIdValue = jTokenId.Value<string>();

                userProfile.Id = string.IsNullOrWhiteSpace(jTokenIdValue) ? Guid.Empty : new Guid(jTokenIdValue);
            }

            var jTokenEmail = jObjectUser["email"];

            if (jTokenEmail is not null)
            {
                var jTokenEmailValue = jTokenEmail.Value<string>();

                if (!string.IsNullOrWhiteSpace(jTokenEmailValue))
                {
                    userProfile.Username = jTokenEmailValue;
                    userProfile.EmailAddress = jTokenEmailValue;
                }
            }

            var jTokenDisplayName = jObjectUser["displayName"];

            if (jTokenDisplayName is not null)
            {
                var jTokenDisplayNameValue = jTokenDisplayName.Value<string>();
                userProfile.DisplayName = string.IsNullOrWhiteSpace(jTokenDisplayNameValue) ? string.Empty : jTokenDisplayNameValue;
            }

            var jTokenEmployeeId = jObjectUser["employeeId"];

            if (jTokenEmployeeId is not null)
            {
                var jTokenEmployeeIdValue = jTokenEmployeeId.Value<string>();
                userProfile.EmployeeId = string.IsNullOrWhiteSpace(jTokenEmployeeIdValue) ? string.Empty : jTokenEmployeeIdValue;
            }

            var jTokenCompanyCode = jObjectUser["companyCode"];

            if (jTokenCompanyCode is not null)
            {
                var jTokenCompanyCodeValue = jTokenCompanyCode.Value<string>();
                userProfile.CompanyCode = string.IsNullOrWhiteSpace(jTokenCompanyCodeValue) ? string.Empty : jTokenCompanyCodeValue;
            }

            var jTokenMobilePhone = jObjectUser["mobilePhone"];

            if (jTokenMobilePhone is not null)
            {
                var jTokenMobilePhoneValue = jTokenMobilePhone.Value<string>();
                userProfile.MobilePhone = string.IsNullOrWhiteSpace(jTokenMobilePhoneValue) ? string.Empty : jTokenMobilePhoneValue;
            }

            var jTokenExtensionAttributes = jObjectUser["extensionAttributes"];

            if (jTokenExtensionAttributes is not null)
            {
                if (jTokenExtensionAttributes.HasValues)
                {
                    var jTokenGeneral = jTokenExtensionAttributes["General"];

                    if (jTokenGeneral is not null)
                    {
                        if (jTokenGeneral.HasValues)
                        {
                            var jTokenEmailFromExtensionAttributes = jTokenGeneral["Email"];

                            if (jTokenEmailFromExtensionAttributes is not null)
                            {
                                var jTokenEmailValue = jTokenEmailFromExtensionAttributes.Value<string>();

                                if (!string.IsNullOrWhiteSpace(jTokenEmailValue))
                                {
                                    userProfile.EmailAddress = jTokenEmailValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        return userProfile;
    }

    private async Task<string> GetAccessToken(CancellationToken cancellationToken)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _idAManUserProfileOptions.TokenUrl,
            ClientId = _idAManUserProfileOptions.ClientId,
            ClientSecret = _idAManUserProfileOptions.ClientSecret,
            Scope = string.Join(' ', _idAManUserProfileOptions.Scopes)
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            throw new InvalidOperationException($"Cannot retreive Access Token from {_idAManUserProfileOptions.TokenUrl}");
        }

        return tokenResponse.AccessToken;
    }
}
