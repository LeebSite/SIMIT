using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Application.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Application.Laporans.Queries.GetLaporans;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

public class LaporansController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateLaporanResponse>> CreateMahasiswa([FromForm] CreateLaporanCommand command)
    {
        return CreatedAtAction(nameof(CreateMahasiswa), await Mediator.Send(command));
    }

    [HttpGet(ApiEndPoint.V1.Laporans.RouteTemplateFor.LaporanId)]
    [Produces(typeof(GetLaporanResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetLaporanResponse>> GetLaporan([FromRoute] Guid laporanId)
    {
        var response = await Mediator.Send(new GetLaporanQuery { LaporanId = laporanId });

        return Ok(response);
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetLaporansLaporan>))]
    public async Task<ActionResult<PaginatedListResponse<GetLaporansLaporan>>> GetLaporans([FromQuery] GetLaporansQuery query)
    {
        return await Mediator.Send(query);
    }

}
