using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbing;
using Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbingList;
using Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbings;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;
//using Pertamina.SIMIT.Shared.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbingsList;
using Pertamina.SIMIT.Application.Pembimbings.Commands.UpdatePembimbing;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbings;
using Pertamina.SIMIT.Application.Pembimbings.Commands.UpdatePembimbings;
using Pertamina.SIMIT.Application.Pembimbings.Commands.DeletePembimbing;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class PembimbingsController : ApiControllerBase
{
    [HttpPost]
    [Consumes(ContentTypes.ApplicationXWwwFormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreatePembimbingResponse>> CreatePembimbing([FromForm] CreatePembimbingCommand command)
    {
        return CreatedAtAction(nameof(CreatePembimbing), await Mediator.Send(command));
    }

    [HttpPut(ApiEndPoint.V1.Pembimbings.RouteTemplateFor.PembimbingId)]
    [Consumes(ContentTypes.ApplicationXWwwFormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdatePembimbing([FromRoute] Guid pembimbingId, [FromForm] UpdatePembimbingCommand command)
    {
        if (pembimbingId != command.PembimbingId)
        {
            throw new MismatchException(nameof(UpdatePembimbingCommand.PembimbingId), pembimbingId, command.PembimbingId);
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost(ApiEndPoint.V1.Pembimbings.RouteTemplateFor.UpdatePembimbings)]
    [Consumes(ContentTypes.ApplicationXWwwFormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UpdatePembimbingsResponse>> UpdatePembimbings([FromForm] UpdatePembimbingsCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete(ApiEndPoint.V1.Pembimbings.RouteTemplateFor.PembimbingId)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePembimbing([FromRoute] Guid pembimbingId)
    {
        await Mediator.Send(new DeletePembimbingCommand { PembimbingId = pembimbingId });

        return NoContent();
    }

    [HttpGet(ApiEndPoint.V1.Pembimbings.RouteTemplateFor.PembimbingId)]
    [Produces(typeof(GetPembimbingResponse))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPembimbingResponse>> GetPembimbing([FromRoute] Guid pembimbingId)
    {
        return await Mediator.Send(new GetPembimbingQuery { PembimbingId = pembimbingId });
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetPembimbingsPembimbing>))]
    public async Task<ActionResult<PaginatedListResponse<GetPembimbingsPembimbing>>> GetPembimbings([FromQuery] GetPembimbingsQuery query)
    {
        return await Mediator.Send(query);
    }

    //[HttpGet(ApiEndPoint.V1.Pembimbings.RouteTemplateFor.List)]
    //[Produces(typeof(ListResponse<GetPembimbingsList>))]
    //public async Task<ActionResult<ListResponse<GetPembimbingsList>>> GetPembimbingsList()
    //{
    //    return await Mediator.Send(new GetPembimbingsListQuery());
    //}

}
