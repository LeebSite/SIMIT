using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Application.Pembimbings.Queries.GetPembimbing;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;

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

    [HttpGet]
    [Produces(typeof(GetPembimbingResponse))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPembimbingResponse>> GetPembimbing([FromRoute] Guid pembimbingId)
    {
        return await Mediator.Send(new GetPembimbingQuery { PembimbingId = pembimbingId });
    }

}
