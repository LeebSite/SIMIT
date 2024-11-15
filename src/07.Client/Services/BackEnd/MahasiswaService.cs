using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswasList;
using RestSharp;
using static Pertamina.SIMIT.Shared.Mahasiswas.Constants.ApiEndpoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;
public class MahasiswaService
{
    private readonly RestClient _restClient;

    public MahasiswaService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {

        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{Mahasiswas.Segment}");
        _restClient.AddUserInfo(userInfo);
    }
    public async Task<ResponseResult<CreateMahasiswaResponse>> CreateMahasiswaAsync(CreateMahasiswaRequest request)

    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreateMahasiswaResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetMahasiswasMahasiswa>>> GetMahasiswasAsync(PaginatedListRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetMahasiswasMahasiswa>>();
    }

    public async Task<ResponseResult<ListResponse<GetMahasiswasList>>> GetMahasiswasListAsync()
    {
        var restRequest = new RestRequest(nameof(Mahasiswas.RouteTemplateFor.List), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetMahasiswasList>>();
    }

    public async Task<ResponseResult<GetMahasiswaResponse>> GetMahasiswaAsync(Guid mahasiswaId)
    {
        var restRequest = new RestRequest(mahasiswaId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetMahasiswaResponse>();
    }

}
