using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;
using RestSharp;
using static Pertamina.SIMIT.Shared.Laporans.Constants.ApiEndPoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;
public class LaporanService
{
    private readonly RestClient _restClient;

    public LaporanService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {

        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{Laporans.Segment}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<CreateLaporanResponse>> CreateLaporanAsync(CreateLaporanRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreateLaporanResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetLaporansLaporan>>> GetLaporansAsync(PaginatedListRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetLaporansLaporan>>();
    }

    public async Task<ResponseResult<GetLaporanResponse>> GetLaporanAsync(Guid laporanId)
    {
        var restRequest = new RestRequest(laporanId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetLaporanResponse>();
    }

}
