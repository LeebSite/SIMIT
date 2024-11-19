using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbings;
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

    //public async Task<ResponseResult<CreatePembimbingResponse>> CreatePembimbingAsync(CreatePembimbingRequest request)
    //{
    //    var restRequest = new RestRequest(string.Empty, Method.Post);
    //    restRequest.AddParameters(request);

    //    var restResponse = await _restClient.ExecuteAsync(restRequest);

    //    return restResponse.ToResponseResult<CreatePembimbingResponse>();
    //}

    public async Task<ResponseResult<CreatePembimbingResponse>> CreatePembimbingAsync(CreatePembimbingRequest request)

    {
        var restRequest = new RestRequest(string.Empty, Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreatePembimbingResponse>();
    }

    public async Task<ResponseResult<SuccessResponse>> UpdatePembimbingAsync(UpdatePembimbingRequest request)
    {
        var restRequest = new RestRequest(request.PembimbingId.ToString(), Method.Put);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<SuccessResponse>();
    }

    public async Task<ResponseResult<UpdatePembimbingsResponse>> UpdatePembimbingsAsync(UpdatePembimbingsRequest request)
    {
        var restRequest = new RestRequest(nameof(Pembimbings.RouteTemplateFor.UpdatePembimbings), Method.Post);
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<UpdatePembimbingsResponse>();
    }

    public async Task<ResponseResult<SuccessResponse>> DeletePembimbingAsync(Guid pembimbingId)
    {
        var restRequest = new RestRequest(pembimbingId.ToString(), Method.Delete);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<SuccessResponse>();
    }

    public async Task<ResponseResult<ListResponse<GetPembimbingsList>>> GetPembimbingsListAsync()
    {
        var restRequest = new RestRequest(nameof(Pembimbings.RouteTemplateFor.List), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetPembimbingsList>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetPembimbingsPembimbing>>> GetPembimbingsAsync(PaginatedListRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetPembimbingsPembimbing>>();
    }

    public async Task<ResponseResult<GetPembimbingResponse>> GetPembimbingAsync(Guid pembimbingId)
    {
        var restRequest = new RestRequest(pembimbingId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetPembimbingResponse>();
    }

}
