using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.LogbookAttachments.Commands.CreateLogbookAttachment;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.LogbookAttachments.CreateLogbookAttachments;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

[ApiVersion(ApiVersioning.V1.Number)]
public class LogbookAttachmentController : ApiControllerBase
{
    [HttpPost]
    [Consumes(ContentTypes.MultipartFormData)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]

    public async Task<ActionResult<CreateLogbookAttachmentResponse>> CreateLogbookAttachment([FromForm] CreateLogbookAttachmentCommand command)
    {
        return CreatedAtAction(nameof(CreateLogbookAttachment), await Mediator.Send(command));
    }
}

