using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswas;
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

    public async Task<ResponseResult<PaginatedListResponse<GetMahasiswasMahasiswa>>> GetMahasiswasAsync(GetMahasiswaRequest request)
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

    public async Task<ResponseResult<SuccessResponse>> UpdateMahasiswaAsync(UpdateMahasiswaRequest request)
    {
        var restRequest = new RestRequest(request.MahasiswaId.ToString(), Method.Put);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<SuccessResponse>();
    }

    public async Task<ResponseResult<UpdateMahasiswasResponse>> UpdateMahasiswasAsync(UpdateMahasiswasRequest request)
    {
        var restRequest = new RestRequest(nameof(Mahasiswas.RouteTemplateFor.UpdateMahasiswas), Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<UpdateMahasiswasResponse>();
    }

    public async Task<ResponseResult<SuccessResponse>> DeleteMahasiswaAsync(Guid mahasiswaId)
    {
        var restRequest = new RestRequest(mahasiswaId.ToString(), Method.Delete);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<SuccessResponse>();
    }

    public async Task<ResponseResult<GetMahasiswasMahasiswa>> GetMahasiswaCountAsync()
    {
        var restRequest = new RestRequest(nameof(Mahasiswas.RouteTemplateFor.Count), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetMahasiswasMahasiswa>();
    }

}
