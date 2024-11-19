using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Application.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;

namespace Pertamina.SIMIT.WebApi.Areas.V1.Controllers;

public class LogbooksController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateLogbookResponse>> CreateLogbook([FromForm] CreateLogbookCommand command)
    {
        return CreatedAtAction(nameof(CreateLogbook), await Mediator.Send(command));
    }
}
