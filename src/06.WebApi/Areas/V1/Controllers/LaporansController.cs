using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Application.Laporans.Commands.UpdateLaporan;
using Pertamina.SIMIT.Application.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Application.Laporans.Queries.GetLaporans;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;
using Pertamina.SIMIT.WebApi.Common.Extensions;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

public class LaporansController : ApiControllerBase
{
    [HttpPost]
    [Consumes(ContentTypes.MultipartFormData)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateLaporanResponse>> CreateLaporan([FromForm] CreateLaporanCommand command)
    {
        return CreatedAtAction(nameof(CreateLaporan), await Mediator.Send(command));
    }

    [HttpGet(ApiEndPoint.V1.Laporans.RouteTemplateFor.Download)]
    [Produces(typeof(GetLaporanResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetLaporan([FromRoute] Guid laporanId)
    {
        var response = await Mediator.Send(new GetLaporanQuery { LaporanId = laporanId });

        return response.AsFile();
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetLaporansLaporan>))]
    public async Task<ActionResult<PaginatedListResponse<GetLaporansLaporan>>> GetLaporans([FromQuery] GetLaporansQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPut(ApiEndPoint.V1.Laporans.RouteTemplateFor.LaporanId)]
    [Consumes(ContentTypes.ApplicationXWwwFormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateLaporan([FromRoute] Guid laporanId, [FromForm] UpdateLaporanCommand command)
    {
        if (laporanId != command.LaporanId)
        {
            throw new MismatchException(nameof(UpdateLaporanCommand.LaporanId), laporanId, command.LaporanId);
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
