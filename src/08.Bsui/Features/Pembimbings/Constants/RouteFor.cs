namespace Pertamina.SIMIT.Bsui.Features.Pembimbings.Constants;

public static class RouteFor
{
    public const string Index = nameof(Pembimbings);

    public static string Details(Guid id)
    {
        return $"{nameof(Pembimbings)}/{nameof(Details)}/{id}";
    }
}
