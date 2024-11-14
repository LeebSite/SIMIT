
using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;
public class MahasiswasController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateMahasiswaResponse>> CreateMahasiswa([FromForm] CreateMahasiswaCommand command)
    {
        return CreatedAtAction(nameof(CreateMahasiswa), await Mediator.Send(command));
    }

    [HttpGet("{mahasiswaId}")]
    [Produces(typeof(GetMahasiswaResponse))]
    public async Task<ActionResult<GetMahasiswaResponse>> GetMahasiswa([FromRoute] Guid mahasiswaId)
    {
        return await Mediator.Send(new GetMahasiswaQuery { MahasiswaId = mahasiswaId });
    }

}
