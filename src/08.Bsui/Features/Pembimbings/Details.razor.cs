using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Components;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Components;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbing;

namespace Pertamina.SIMIT.Bsui.Features.Pembimbings;
public partial class Details
{
    //[CascadingParameter]
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
            CommonBreadcrumbFor.Active(DisplayTextFor.DataPembimbing)
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

    private async Task ShowDialogEdit()
    {
        var request = new UpdatePembimbingRequest
        {
            PembimbingId = _pembimbing.Id,
            Nama = _pembimbing.Nama,
            Nip = _pembimbing.Nip,
            Jabatan = _pembimbing.Jabatan
        };

        var parameters = new DialogParameters
        {
            { nameof(DialogEdit.Request), request }
        };

        var dialog = _dialogService.Show<DialogEdit>($"{CommonDisplayTextFor.Edit} {DisplayTextFor.Pembimbing}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Pembimbing, _pembimbing.Nama, CommonDisplayTextFor.Updated));

            await Reload();
        }
    }

    private async Task ShowDialogDelete()
    {
        var dialog = _dialogService.Show<DialogDelete>($"{CommonDisplayTextFor.Delete} {DisplayTextFor.Pembimbing} {_pembimbing.Nama}");
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var response = await _pembimbingService.DeletePembimbingAsync(_pembimbing.Id);

            if (response.Error is not null)
            {
                _snackbar.AddErrors(response.Error.Details);

                return;
            }

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Pembimbing, _pembimbing.Nama, CommonDisplayTextFor.Deleted));
            _navigationManager.NavigateTo(RouteFor.Index);
        }
    }
}
