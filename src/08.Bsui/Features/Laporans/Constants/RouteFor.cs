using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Bsui.Features.Laporans.Constants;

public class RouteFor
{
    public const string Index = nameof(Laporans);

    public static string Details(Guid id)
    {
        return $"{nameof(Laporans)}/{nameof(Details)}/{id}";
    }

    public static readonly string DokumenMahasiswa = nameof(DokumenMahasiswa).SplitWords();
}
