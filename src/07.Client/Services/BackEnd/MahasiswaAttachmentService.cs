using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Queries.GetMahasiswaAttachmentFile;
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

    public async Task<ResponseResult<GetMahasiswaAttachmentFileResponse>> GetMahasiswaAttachmentFileAsync(Guid mahasiswaAttachmentId)
    {
        var restRequest = new RestRequest($"Downloads/{mahasiswaAttachmentId}", Method.Get);
        //var restRequest = new RestRequest($"{nameof(MahasiswaAttachments.RouteTemplateFor.Download)}/{mahasiswaAttachmentId}", Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetMahasiswaAttachmentFileResponse>();
    }
    //public async Task<string> ReadAsBase64Async(Guid mahasiswaAttachmentId)
    //{
    //    var response = await GetMahasiswaAttachmentFileAsync(mahasiswaAttachmentId);

    //    if (response.Error != null)
    //    {
    //        throw new Exception($"Error retrieving file: {response.Error.Details}");
    //    }

    //    var fileBytes = Convert.FromBase64String(response.Result.Content);
    //    return Convert.ToBase64String(fileBytes);
    //}

}
