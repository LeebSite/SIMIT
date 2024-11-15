using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas.Constants;

public static class RouteFor
{
    public const string Index = nameof(Mahasiswas);

    public static string Details(Guid id)
    {
        return $"{nameof(Mahasiswas)}/{nameof(Details)}/{id}";
    }

    public static readonly string ListMahasiswa = nameof(ListMahasiswa).SplitWords();
}
