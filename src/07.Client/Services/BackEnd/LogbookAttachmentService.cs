using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.LogbookAttachments.Commands.CreateLogbookAttachment;
using Pertamina.SIMIT.Shared.LogbookAttachments.CreateLogbookAttachments;
using Pertamina.SIMIT.Shared.LogbookAttachments.Queries.GetLogbookAttachmentFile;
using RestSharp;
using static Pertamina.SIMIT.Shared.LogbookAttachments.Constants.ApiEndpoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;
public class LogbookAttachmentService
{
    private readonly RestClient _restClient;

    public LogbookAttachmentService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{LogbookAttachments.Segment}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<CreateLogbookAttachmentResponse>> CreateLogbookAttachmentAsync(CreateLogbookAttachmentRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post) { AlwaysMultipartFormData = true };
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreateLogbookAttachmentResponse>();
    }

    public async Task<ResponseResult<GetLogbookAttachmentFileResponse>> GetLogbookAttachmentFileAsync(Guid logbookAttachmentId)
    {
        var restRequest = new RestRequest($"{nameof(LogbookAttachments.RouteTemplateFor.Download)}/{logbookAttachmentId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetLogbookAttachmentFileResponse>();
    }
}
