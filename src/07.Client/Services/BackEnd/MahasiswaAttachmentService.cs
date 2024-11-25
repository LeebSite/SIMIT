using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using RestSharp;
using static Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants.ApiEndpoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;

public class MahasiswaAttachmentService
{
    private readonly RestClient _restClient;
    public MahasiswaAttachmentService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{MahasiswaAttachments.Segment}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<CreateMahasiswaAttachmentResponse>> CreateMahasiswaAttachmentAsync(CreateMahasiswaAttachmentRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post) { AlwaysMultipartFormData = true };
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreateMahasiswaAttachmentResponse>();
    }
}
