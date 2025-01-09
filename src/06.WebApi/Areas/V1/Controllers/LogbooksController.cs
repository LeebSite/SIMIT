using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Logbooks.Commands.ApprovalLogbook;
using Pertamina.SIMIT.Application.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbook;
using Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooks;
using Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooksListQuery;
using Pertamina.SIMIT.Application.Logbooks.Queries.GetLogbooksQuery;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.ApprovalLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooksList;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

public class LogbooksController : ApiControllerBase
{
    [HttpPost]
    [Consumes(ContentTypes.MultipartFormData)]
    [DisableRequestSizeLimit]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateLogbookResponse>> CreateLogbook([FromForm] CreateLogbookCommand command)
    {
        return CreatedAtAction(nameof(CreateLogbook), await Mediator.Send(command));
    }

    [HttpGet(ApiEndPoint.V1.Logbooks.RouteTemplateFor.ByLogbookId)]
    [Produces(typeof(GetLogbookResponse))]
    public async Task<ActionResult<GetLogbookResponse>> GetLogbook([FromRoute] Guid logbookId)
    {
        return await Mediator.Send(new GetLogbookQuery { LogbookId = logbookId });
    }

    [HttpGet(ApiEndPoint.V1.Logbooks.RouteTemplateFor.ByMahasiswaId)]
    [Produces(typeof(PaginatedListResponse<GetLogbooksLogbook>))]
    public async Task<ActionResult<PaginatedListResponse<GetLogbooksLogbook>>> GetLogbookbById([FromRoute] Guid mahasiswaId, [FromQuery] GetLogbooksByIdQuery request)
    {
        var query = new GetLogbooksByIdQuery
        {
            MahasiswaId = mahasiswaId,
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder
        };

        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndPoint.V1.Logbooks.RouteTemplateFor.List)]
    [Produces(typeof(ListResponse<GetLogbooksList>))]
    public async Task<ActionResult<ListResponse<GetLogbooksList>>> GetLogbooksList()
    {
        return await Mediator.Send(new GetLogbooksListQuery());
    }

    [HttpGet(ApiEndPoint.V1.Logbooks.RouteTemplateFor.Approval)]
    [Produces(typeof(PaginatedListResponse<GetLogbooksLogbook>))]
    public async Task<ActionResult<PaginatedListResponse<GetLogbooksLogbook>>> GetLogbooks([FromQuery] GetLogbooksApprovalQuery request)
    {
        var query = new GetLogbooksApprovalQuery
        {
            Page = request.Page,
            PageSize = request.PageSize,
            SearchText = request.SearchText,
            SortField = request.SortField,
            SortOrder = request.SortOrder,
            User = request.User
        };

        return await Mediator.Send(query);
    }

    [HttpPost(ApiEndPoint.V1.Logbooks.RouteTemplateFor.ApproveSingle)]
    public async Task<IActionResult> ApproveSingleLogbook([FromRoute] Guid logbookId)
    {
        if (logbookId == Guid.Empty)
        {
            return BadRequest("Logbook ID cannot be empty.");
        }

        var command = new ApproveSingleLogbookCommand
        {
            LogbookId = logbookId,
            Approval = true
        };
        await Mediator.Send(command);
        return NoContent();

    }

    [HttpPost(ApiEndPoint.V1.Logbooks.RouteTemplateFor.ApproveMultiple)]
    public async Task<ActionResult<ApproveLogbookResponse>> ApproveMultipleLogbooks([FromBody] ApproveMultiLogbooksRequest request)
    {
        if (request == null || request.Logbooks.Count == 0)
        {
            return BadRequest("Logbook list cannot be empty.");
        }

        var response = await Mediator.Send(new ApproveMultiLogbooksCommand
        {
            Logbooks = request.Logbooks
        });

        return Ok(response);
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetLogbooksLogbook>))]
    public async Task<ActionResult<PaginatedListResponse<GetLogbooksLogbook>>> GetLogbooks([FromQuery] GetLogbooksQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndPoint.V1.Logbooks.RouteTemplateFor.Count)]
    public async Task<IActionResult> GetLogbooksPerMonth()
    {
        var result = await Mediator.Send(new GetLogbooksPerMonthQuery());
        return Ok(result);
    }

}
