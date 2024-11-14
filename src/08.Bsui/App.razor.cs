using Microsoft.AspNetCore.Components;

namespace Pertamina.SIMIT.Bsui;

public partial class App
{
    [Parameter]
    public InitialApplicationState InitialApplicationState { get; set; } = default!;

    protected override void OnInitialized()
    {
        _userInfo.IpAddress = InitialApplicationState.IpAddress;
    }
}
