using MudBlazor;
using Pertamina.SIMIT.Shared.Audits.Constants;

namespace Pertamina.SIMIT.Bsui.Features.Audits.Constants;

public static class BreadcrumbFor
{
    public static readonly BreadcrumbItem Index = new(DisplayTextFor.Audits, href: RouteFor.Index);
}
