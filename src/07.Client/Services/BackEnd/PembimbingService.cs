using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbingsList;
using RestSharp;
using static Pertamina.SIMIT.Shared.Pembimbings.Constants.ApiEndPoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;

public class PembimbingService
{
    private readonly RestClient _restClient;

    public PembimbingService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {
        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{Pembimbings.Segment}");
        _restClient.AddUserInfo(userInfo);
    }

    public async Task<ResponseResult<CreatePembimbingResponse>> CreatePembimbingAsync(CreatePembimbingRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreatePembimbingResponse>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetPembimbingsPembimbing>>> GetPembimbingAsync(PaginatedListRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetPembimbingsPembimbing>>();
    }

    public async Task<ResponseResult<ListResponse<GetPembimbingsList>>> GetPembimbingsListAsync()
    {
        var restRequest = new RestRequest(nameof(Pembimbings.RouteTemplateFor.List), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetPembimbingsList>>();
    }

    public async Task<ResponseResult<GetPembimbingResponse>> GetPembimbingAsync(Guid pembimbingId)
    {
        var restRequest = new RestRequest(pembimbingId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetPembimbingResponse>();
    }
    //public async Task<ResponseResult<SuccessResponse>>
}
