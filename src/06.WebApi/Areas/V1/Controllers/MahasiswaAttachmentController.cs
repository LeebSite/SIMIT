using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using Pertamina.SIMIT.Application.MahasiswaAttachments.Queries.GetMahasiswaAttachmentFile;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Queries.GetMahasiswaAttachmentFile;
using Pertamina.SIMIT.WebApi.Common.Extensions;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class MahasiswaAttachmentController : ApiControllerBase
{
    [HttpPost]
    [Consumes(ContentTypes.MultipartFormData)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateMahasiswaAttachmentResponse>> CreateMahasiswaAttachment([FromForm] CreateMahasiswaAttachmentCommand command)
    {
        return CreatedAtAction(nameof(CreateMahasiswaAttachment), await Mediator.Send(command));
    }

    [HttpGet(ApiEndpoint.V1.MahasiswaAttachments.RouteTemplateFor.Download)]
    [Produces(typeof(GetMahasiswaAttachmentFileResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetMahasiswaAttachmentFile([FromRoute] Guid mahasiswaAttachmentId)
    {
        var response = await Mediator.Send(new GetMahasiswaAttachmentFileQuery { MahasiswaAttachmentId = mahasiswaAttachmentId });

        return response.AsFile();
    }

    //[HttpGet(ApiEndPoint.V1.Laporans.RouteTemplateFor.Download)]
    //[Produces(typeof(GetLaporanResponse))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult> GetLaporan([FromRoute] Guid laporanId)
    //{
    //    var response = await Mediator.Send(new GetLaporanQuery { LaporanId = laporanId });

    //    return response.AsFile();
    //}

}
