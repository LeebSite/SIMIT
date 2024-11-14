using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Services.External.Location.Models;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Bsui.Common.Pages;

public partial class About
{
    private const string Cookie = nameof(Cookie);

    private List<BreadcrumbItem> _breadcrumbItems = new();
    private GeolocationDetails _userGeolocationDetails = default!;

    private readonly Dictionary<string, string[]> _requestHeaders = new();
    private readonly Dictionary<string, string[]> _responseHeaders = new();
    private ConnectionInfo _connectionInfo = default!;

    protected override async Task OnInitializedAsync()
    {
        SetupBreadcrumb();

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            _connectionInfo = httpContext.Connection;

            foreach (var header in httpContext.Request.Headers)
            {
                if (!header.Key.Equals("Cookie"))
                {
                    _requestHeaders.Add(header.Key, header.Value.ToArray());
                }
            }

            foreach (var header in httpContext.Response.Headers)
            {
                _responseHeaders.Add(header.Key, header.Value.ToArray());
            }
        }

        if (_geolocationOptions.Value.Enabled)
        {
            if (_userInfo.Geolocation is not null)
            {
                _userGeolocationDetails = await _locationExternalService.GetGeolocationDetails(_userInfo.Geolocation);

                StateHasChanged();
            }
        }
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(CommonDisplayTextFor.About)
        };
    }
}
