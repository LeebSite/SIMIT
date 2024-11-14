﻿using Microsoft.AspNetCore.Http;

namespace Pertamina.SIMIT.Client.Common.Extensions;

public static class FormFileExtensions
{
    public static byte[] ToBytes(this IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();

        formFile.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}
