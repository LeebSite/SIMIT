using Microsoft.Extensions.Options;
using Pertamina.SIMIT.Client.Common.Extensions;
using Pertamina.SIMIT.Client.Services.UserInfo;
using Pertamina.SIMIT.Shared.Common.Requests;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooksList;
using RestSharp;
using static Pertamina.SIMIT.Shared.Logbooks.Constants.ApiEndPoint.V1;

namespace Pertamina.SIMIT.Client.Services.BackEnd;
public class LogbookService
{
    private readonly RestClient _restClient;

    public LogbookService(IOptions<BackEndOptions> backEndServiceOptions, UserInfoService userInfo)
    {

        _restClient = new RestClient($"{backEndServiceOptions.Value.BaseUrl}/{Logbooks.Segment}");
        _restClient.AddUserInfo(userInfo);
    }
    public async Task<ResponseResult<CreateLogbookResponse>> CreateLogbookAsync(CreateLogbookRequest request)

    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
        {
            AlwaysMultipartFormData = true
        };
        restRequest.AddParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<CreateLogbookResponse>();
    }
    public async Task<ResponseResult<PaginatedListResponse<GetLogbooksLogbook>>> GetLogbooksAsync(PaginatedListRequest request)
    {
        var restRequest = new RestRequest(string.Empty, Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetLogbooksLogbook>>();
    }

    public async Task<ResponseResult<PaginatedListResponse<GetLogbooksLogbook>>> GetLogbooksByIdAsync(PaginatedListRequest request, Guid mahasiswaId)
    {
        var restRequest = new RestRequest($"V1/Logbooks/ByMahasiswaId/{mahasiswaId}", Method.Get);
        restRequest.AddQueryParameters(request);

        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<PaginatedListResponse<GetLogbooksLogbook>>();
    }

    public async Task<ResponseResult<ListResponse<GetLogbooksList>>> GetLogbooksListAsync()
    {
        var restRequest = new RestRequest(nameof(Logbooks.RouteTemplateFor.List), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<ListResponse<GetLogbooksList>>();
    }
    public async Task<ResponseResult<GetLogbookResponse>> GetLogbookAsync(Guid logbookId)
    {
        var restRequest = new RestRequest(logbookId.ToString(), Method.Get);
        var restResponse = await _restClient.ExecuteAsync(restRequest);

        return restResponse.ToResponseResult<GetLogbookResponse>();
    }

}
