using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;

namespace Pertamina.SIMIT.Bsui.Features.Pembimbings;
public partial class Details
{
    [CascadingParameter]
    [Parameter]
    public Guid PembimbingId { get; set; }
    private bool _isLoading;
    private ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private GetPembimbingResponse _pembimbing = default!;

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnParametersSetAsync()
    {
        await Reload();
    }

    private async Task Reload()
    {
        SetupBreadcrumb();

        await GetPembimbing();

        if (_pembimbing is null)
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));

            return;
        }

        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_pembimbing.Nama));
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            BreadCrumbFor.Index
        };
    }

    private async Task GetPembimbing()
    {
        _isLoading = true;

        var response = await _pembimbingService.GetPembimbingAsync(PembimbingId);

        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        _pembimbing = response.Result!;
    }
}
