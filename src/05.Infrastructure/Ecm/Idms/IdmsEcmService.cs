using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.Ecm;
using Pertamina.SIMIT.Application.Services.Ecm.Models.UploadContent;
using Pertamina.SIMIT.Infrastructure.Ecm.Idms.Models;
using Pertamina.SIMIT.Shared.Common.Constants;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Ecm.Idms;

public class IdmsEcmService : IEcmService
{
    private readonly ILogger<IdmsEcmService> _logger;
    private readonly IdmsEcmOptions _idmsEcmOptions;
    private readonly RestClient _restClient;

    public IdmsEcmService(ILogger<IdmsEcmService> logger, IOptions<IdmsEcmOptions> idmsEcmOptions)
    {
        _logger = logger;
        _idmsEcmOptions = idmsEcmOptions.Value;
        _restClient = new RestClient(_idmsEcmOptions.BaseUrl);
    }

    public async Task<UploadContentResponse> UploadContentAsync(UploadContentRequest uploadContentModel, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessToken(cancellationToken);

        var restRequest = new RestRequest(_idmsEcmOptions.Endpoints.Documents, Method.Post)
        {
            AlwaysMultipartFormData = true
        };

        var createDocumentRequest = CreateDocumentRequest.ToCreateDocumentRequest(uploadContentModel, new Guid(_idmsEcmOptions.ApplicationId));

        restRequest.AddHeader(HttpHeaderName.Authorization, $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}");
        restRequest.AddParameter(nameof(CreateDocumentRequest.ClientApplicationId), createDocumentRequest.ClientApplicationId);
        restRequest.AddParameter(nameof(CreateDocumentRequest.CompanyCode), createDocumentRequest.CompanyCode);
        restRequest.AddParameter(nameof(CreateDocumentRequest.DocumentCategoryName), createDocumentRequest.DocumentCategoryName);
        restRequest.AddParameter(nameof(CreateDocumentRequest.JrdpYear), createDocumentRequest.JrdpYear);
        restRequest.AddParameter(nameof(CreateDocumentRequest.TopicCode), createDocumentRequest.TopicCode);
        restRequest.AddFile(nameof(CreateDocumentRequest.DocumentFile), uploadContentModel.FileContent, uploadContentModel.FileName, contentType: uploadContentModel.FileContentType);

        var fullRequestUri = _restClient.BuildUri(restRequest);

        _logger.LogInformation("Uploading document to {ServerUrl} with metadata: {@DocumentMetadata}", fullRequestUri, createDocumentRequest);

        var restResponse = await _restClient.ExecuteAsync<CreateDocumentResponse>(restRequest, cancellationToken);

        if (!restResponse.IsSuccessful)
        {
            var exception = new Exception(restResponse.ErrorMessage);

            _logger.LogError(exception, "Error in uploading document to {ServerUrl} with metadata: {@DocumentMetadata}", fullRequestUri, createDocumentRequest);

            throw exception;
        }

        if (restResponse.Data is null)
        {
            throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(CreateDocumentResponse)}.");
        }

        return new UploadContentResponse
        {
            ContentId = restResponse.Data.DocumentId
        };
    }

    private async Task<string> GetAccessToken(CancellationToken cancellationToken)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _idmsEcmOptions.TokenUrl,
            ClientId = _idmsEcmOptions.ClientId,
            ClientSecret = _idmsEcmOptions.ClientSecret,
            Scope = $"api.auth {_idmsEcmOptions.ScopeId}"
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            throw new InvalidOperationException($"Cannot retreive Access Token from {_idmsEcmOptions.TokenUrl}. Error: {tokenResponse.Error}. Error Description: {tokenResponse.ErrorDescription}.");
        }

        return tokenResponse.AccessToken;
    }
}
