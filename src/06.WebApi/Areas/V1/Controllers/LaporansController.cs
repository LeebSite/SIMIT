using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

public class LaporansController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateLaporanResponse>> CreateMahasiswa([FromForm] CreateLaporanCommand command)
    {
        return CreatedAtAction(nameof(CreateMahasiswa), await Mediator.Send(command));
    }
}
