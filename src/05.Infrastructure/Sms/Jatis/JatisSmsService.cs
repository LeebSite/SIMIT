using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Application.Services.BackgroundJob;
using Pertamina.SIMIT.Application.Services.Sms;
using Pertamina.SIMIT.Application.Services.Sms.Models.SendSms;
using RestSharp;

namespace Pertamina.SIMIT.Infrastructure.Sms.Jatis;

public class JatisSmsService : ISmsService
{
    private readonly ILogger<JatisSmsService> _logger;
    private readonly JatisSmsOptions _jatisSmsOptions;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly RestClient _restClient;

    public JatisSmsService(ILogger<JatisSmsService> logger, IOptions<JatisSmsOptions> jatisSmsOptions, IBackgroundJobService backgroundJobService)
    {
        _logger = logger;
        _jatisSmsOptions = jatisSmsOptions.Value;
        _backgroundJobService = backgroundJobService;
        _restClient = new RestClient(_jatisSmsOptions.Url);
    }

    public async Task SendSmsAsync(SendSmsRequest smsModel)
    {
        await _backgroundJobService.RunJob(() => SendAsync(smsModel));
    }

    public async Task SendAsync(SendSmsRequest smsModel)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddParameter("userid", _jatisSmsOptions.UserId);
        restRequest.AddParameter("password", _jatisSmsOptions.Password);
        restRequest.AddParameter("sender", _jatisSmsOptions.Sender);
        restRequest.AddParameter("division", _jatisSmsOptions.Division);
        restRequest.AddParameter("uploadby", _jatisSmsOptions.UploadBy);
        restRequest.AddParameter("batchname", smsModel.Id);
        restRequest.AddParameter("msisdn", smsModel.To);
        restRequest.AddParameter("message", smsModel.Message);
        restRequest.AddParameter("channel", "2");

        try
        {
            var restResponse = await _restClient.ExecuteAsync(restRequest);

            _logger.LogInformation("Executing method {MethodName} with request {@Request} and response {@Response}", nameof(SendAsync), restRequest, restResponse.Content);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error in executing method {MethodName} with request {@Request}", nameof(SendAsync), restRequest);

            throw;
        }
    }
}
