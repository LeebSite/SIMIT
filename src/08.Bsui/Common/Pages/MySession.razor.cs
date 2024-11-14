﻿using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Services.Authentication.Constants;

namespace Pertamina.SIMIT.Bsui.Common.Pages;

public partial class MySession
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudMessageBox _messageBoxAccessToken = default!;
    private bool _inProductionEnvironment;
    private string _accessToken = default!;

    protected override void OnInitialized()
    {
        SetupBreadcrumb();

        _inProductionEnvironment = _webHostEnvironment.IsInEnvironment(EnvironmentNames.Production);
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(AuthenticationDisplayTextFor.MySession)
        };
    }

    private async void ShowMessageBoxAccessToken()
    {
        await _messageBoxAccessToken.Show();
    }
}
