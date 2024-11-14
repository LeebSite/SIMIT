using System.Timers;
using Darnton.Blazor.DeviceInterop.Geolocation;
using Pertamina.SIMIT.Base.ValueObjects;
using Pertamina.SIMIT.Bsui.Services.Authentication;
using Pertamina.SIMIT.Bsui.Services.Telemetry;
using Timer = System.Timers.Timer;

namespace Pertamina.SIMIT.Bsui.Layouts;

public partial class MainLayout
{
    private readonly Timer _timerForRefreshTokens = new();

    private bool _firstTimeRender;
    private bool _drawerOpen;
    private bool _usingApplicationInsights;
    private bool _usingAuthentication;
    private bool _usingGeolocation;
    private GeolocationResult? _geolocationResult;
    private bool _readyToRenderBody;

    protected override void OnInitialized()
    {
        _timerForRefreshTokens.Interval = TimeSpan.FromSeconds(_authenticationOptions.Value.RefreshRateInSeconds).TotalMilliseconds;
        _timerForRefreshTokens.Elapsed += RefreshTokens;
        _timerForRefreshTokens.AutoReset = false;

        _usingApplicationInsights = _telemetryOptions.Value.Provider is TelemetryProvider.ApplicationInsights;
        _usingAuthentication = _authenticationOptions.Value.Provider is not AuthenticationProvider.None;
        _usingGeolocation = _geolocationOptions.Value.Enabled;

        _firstTimeRender = true;
        _drawerOpen = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ((AuthorizedAuthenticationStateProvider)_authenticationStateProvider).LoadIdentity();

            if (_usingGeolocation)
            {
                _geolocationResult = await _geolocationService.GetCurrentPosition();

                if (_geolocationResult.IsSuccess)
                {
                    var coordinates = _geolocationResult.Position.Coords;

                    _userInfo.Geolocation = new Geolocation(coordinates.Latitude, coordinates.Longitude, coordinates.Accuracy);
                }
            }

            _firstTimeRender = false;
            _timerForRefreshTokens.Start();

            StateHasChanged();
        }
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void RefreshTokens(object? sender, ElapsedEventArgs e)
    {
        InvokeAsync(async () => await ((AuthorizedAuthenticationStateProvider)_authenticationStateProvider).RefreshTokensAsync());

        _timerForRefreshTokens.Stop();
        _timerForRefreshTokens.Start();
    }
}
