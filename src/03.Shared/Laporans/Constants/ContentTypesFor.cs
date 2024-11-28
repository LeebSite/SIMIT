using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Shared.Laporans.Constants;
public class ContentTypesFor
{
    public static class LaporanFile
    {
        public const string Value = @"application/x-zip-compressed";

        public const string AllowedFileExtensions = @".zip";

        public static readonly string[] List = new string[]
        {
                ContentTypes.ApplicationZip
        };
    }
}
