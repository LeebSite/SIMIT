using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.WebApi.Common.Extensions;

public static class FileResponseExtension
{
    public static FileContentResult AsFile(this FileResponse fileResponse)
    {
        return new FileContentResult(fileResponse.Content, fileResponse.ContentType)
        {
            FileDownloadName = fileResponse.FileName
        };
    }
}
