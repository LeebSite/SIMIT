using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswa;
using Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswasListQuery;
using Pertamina.SIMIT.Application.Mahasiswas.Queries.GetMahasiswasQuery;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswasList;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;
public class MahasiswasController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateMahasiswaResponse>> CreateMahasiswa([FromForm] CreateMahasiswaCommand command)
    {
        return CreatedAtAction(nameof(CreateMahasiswa), await Mediator.Send(command));
    }

    [HttpGet(ApiEndpoint.V1.Mahasiswas.RouteTemplateFor.MahasiswaId)]
    [Produces(typeof(GetMahasiswaResponse))]
    public async Task<ActionResult<GetMahasiswaResponse>> GetMahasiswa([FromRoute] Guid mahasiswaId)
    {
        return await Mediator.Send(new GetMahasiswaQuery { MahasiswaId = mahasiswaId });
    }

    [HttpGet]
    [Produces(typeof(PaginatedListResponse<GetMahasiswasMahasiswa>))]
    public async Task<ActionResult<PaginatedListResponse<GetMahasiswasMahasiswa>>> GetMahasiswas([FromQuery] GetMahasiswasQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet(ApiEndpoint.V1.Mahasiswas.RouteTemplateFor.List)]
    [Produces(typeof(ListResponse<GetMahasiswasList>))]
    public async Task<ActionResult<ListResponse<GetMahasiswasList>>> GetMahasiswasList()
    {
        return await Mediator.Send(new GetMahasiswasListQuery());
    }

}
