using Pertamina.SIMIT.Bsui.Services.Authorization;
using Pertamina.SIMIT.Bsui.Services.Authorization.IdAMan;

namespace Pertamina.SIMIT.Bsui.Common.Pages.Errors;

public partial class CannotReachAuthorizationProvider
{
    private string _authorizationProviderUrl = default!;

    protected override void OnInitialized()
    {
        switch (_authorizationOptions.Value.Provider)
        {
            case AuthorizationProvider.None:
                break;
            case AuthorizationProvider.IdAMan:
                var idAManAuthorizationOptions = configuration.GetSection(IdAManAuthorizationOptions.SectionKey).Get<IdAManAuthorizationOptions>();
                _authorizationProviderUrl = idAManAuthorizationOptions.BaseUrl;
                break;
            default:
                break;
        }
    }
}
