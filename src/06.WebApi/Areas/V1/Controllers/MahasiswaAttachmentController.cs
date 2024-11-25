using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;

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
}
