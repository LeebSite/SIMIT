using Pertamina.SIMIT.Bsui.Services.UI.MudBlazorUI;

namespace Pertamina.SIMIT.Bsui.Services.UI;

public static class DependencyInjection
{
    public static IServiceCollection AddUIService(this IServiceCollection services)
    {
        services.AddMudBlazorUIService();

        return services;
    }
}
