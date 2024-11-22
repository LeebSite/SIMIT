using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Bsui.Features.Logbooks.Constants;

public static class RouteFor
{
    public const string Index = nameof(Logbooks);

    public static string Details(Guid id)
    {
        return $"{nameof(Logbooks)}/{nameof(Details)}/{id}";
    }

    public static readonly string ListLogbook = nameof(ListLogbook).SplitWords();
}
