using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;
using IdentityModel.Client;
using Pertamina.SIMIT.Infrastructure.Email.EmailBlast.Models;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Email.EmailBlast;

public class EmailBlastEmailSender : ISender
{
    private const string Authorization = nameof(Authorization);
    private const string Bearer = nameof(Bearer);
    private const string To = nameof(To);
    private const string Cc = nameof(Cc);
    private const string Bcc = nameof(Bcc);
    private const char Comma = ',';

    private readonly EmailBlastEmailOptions _options;
    private readonly RestClient _restClient;

    public EmailBlastEmailSender(EmailBlastEmailOptions emailBlastEmailOptions)
    {
        _options = emailBlastEmailOptions;
        _restClient = new RestClient(emailBlastEmailOptions.BaseUrl);
    }

    public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
    {
        return Task.Run(() => SendAsync(email, token)).Result;
    }

    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = default)
    {
        var emailData = email.Data;
        var tos = emailData.ToAddresses.Select(x => x.EmailAddress);
        var cces = emailData.CcAddresses.Select(x => x.EmailAddress);
        var bcces = emailData.BccAddresses.Select(x => x.EmailAddress);

        var toAddresses = string.Join(Comma, tos);
        var ccAddresses = string.Join(Comma, cces);
        var bccAddresses = string.Join(Comma, bcces);

        var restRequest = new RestRequest(_options.Endpoints.SendEmailWithoutTemplate, Method.Post)
        {
            AlwaysMultipartFormData = true
        };

        var accessToken = await GetAccessToken();
        restRequest.AddHeader(Authorization, $"{Bearer} {accessToken}");
        restRequest.AddParameter(nameof(EmailBlastEmailOptions.AppId), _options.AppId);
        restRequest.AddParameter(nameof(EmailBlastEmailOptions.AppSecret), _options.AppSecret);
        restRequest.AddParameter(To, toAddresses);
        restRequest.AddParameter(Cc, ccAddresses);
        restRequest.AddParameter(Bcc, bccAddresses);
        restRequest.AddParameter(nameof(emailData.Subject), emailData.Subject);
        restRequest.AddParameter(nameof(emailData.Body), emailData.Body);

        var restResponse = await _restClient.ExecuteAsync<SendEmailWithoutTemplateResponse>(restRequest);

        if (!restResponse.IsSuccessful)
        {
            throw new Exception($"Unable to send e-mail using EmailBlast ({_restClient.BuildUri(restRequest)}). Error message: {restResponse.ErrorMessage}");
        }

        if (restResponse.Data is null)
        {
            throw new Exception($"Failed to deserialize JSON content into {nameof(SendEmailWithoutTemplateResponse)}.");
        }

        var sendEmailWithoutTemplateResponse = restResponse.Data;

        if (sendEmailWithoutTemplateResponse.EmailRequestIds.Any())
        {
            return new SendResponse
            {
                MessageId = sendEmailWithoutTemplateResponse.EmailRequestIds.First().ToString()
            };
        }

        return new SendResponse();
    }

    private async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
    {
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _options.IdAManTokenUrl,
            ClientId = _options.IdAManClientId.ToString(),
            ClientSecret = _options.IdAManClientSecret.ToString(),
            Scope = _options.IdAManScopeId
        };

        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            throw new InvalidOperationException($"Cannot retreive Access Token from {_options.IdAManTokenUrl}");
        }

        return tokenResponse.AccessToken;
    }
}
