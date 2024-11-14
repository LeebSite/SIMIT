using Microsoft.AspNetCore.Components.Forms;

namespace Pertamina.SIMIT.Bsui.Common.Extensions;

public static class IBrowserFileExtension
{
    public static FormFile ToFormFile(this IBrowserFile browserFile, MemoryStream stream, string parameterName)
    {
        return new FormFile(stream, 0, stream.Length, parameterName, browserFile.Name)
        {
            Headers = new HeaderDictionary(),
            ContentType = browserFile.ContentType
        };
    }
}
