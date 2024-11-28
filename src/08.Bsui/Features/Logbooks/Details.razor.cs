using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Features.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;

namespace Pertamina.SIMIT.Bsui.Features.Logbooks;

public partial class Details
{
    [Parameter]
    public Guid MahasiswaId { get; set; }
    public Guid LogbookId { get; set; }

    private bool _isLoading;
    private ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private GetLogbookResponse _logbook = default!;

    protected override async Task OnParametersSetAsync()
    {
        await Reload();
    }
    private async Task Reload()
    {
        SetupBreadcrumb();

        await GetLogbook();

        if (_logbook is null)
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));

            return;
        }

        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_logbook.MahasiswaNim));
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            BreadcrumbFor.Index
        };
    }

    private async Task GetLogbook()
    {
        _isLoading = true;
        _error = null;

        try
        {
            var response = await _logbookService.GetLogbookAsync(LogbookId);
            _isLoading = false;

            if (response.Error != null)
            {
                _error = response.Error;
                return;
            }

            _logbook = response.Result ?? new GetLogbookResponse(); // Menghindari null
        }
        catch (Exception ex)
        {
            _isLoading = false;
        }
    }

}
