using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Components;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbings;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;

namespace Pertamina.SIMIT.Bsui.Features.Pembimbings;
public partial class Index
{
    private ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetPembimbingsPembimbing> _tablePembimbings = new();
    private string? _searchKeyword;
    private GetPembimbingsPembimbing _pembimbingBeforeEdited = new();
    private List<UpdatePembimbingsPembimbing> _editedPembimbings = new();
    //private List<Update>

    protected override async Task OnInitializedAsync()
    {
        SetupBreadcrumb();
    }

    //protected override async Task OnAfterRenderAsync(bool firstRender)
    //{
    //    _user = (await AuthenticationStateTask).User;
    //}

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(DisplayTextFor.Pembimbings)
        };
    }

    private async Task<TableData<GetPembimbingsPembimbing>> ReloadTablePembimbings(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetPembimbingsPembimbing>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _pembimbingService.GetPembimbingsAsync(request);

        _error = response.Error;

        if (response.Result is null)
        {
            return tableData;
        }

        StateHasChanged();

        return response.Result.ToTableData();
    }

    private async Task OnSearch(string keyword)
    {
        _searchKeyword = keyword.Trim();

        await _tablePembimbings.ReloadServerData();
    }

    private async Task ShowDialogAdd()
    {
        var request = new CreatePembimbingRequest();

        var parameters = new DialogParameters
    {
        { nameof(DialogAdd.Request), request }
    };

        var dialog = _dialogService.Show<DialogAdd>($"{CommonDisplayTextFor.Add} {DisplayTextFor.Pembimbing}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var id = (Guid)result.Data;

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Pembimbing, CommonDisplayTextFor.Created));
            _navigationManager.NavigateTo(RouteFor.Index, forceLoad: true);
        }
    }

    private void EditCommit(object row)
    {
        var pembimbing = (GetPembimbingsPembimbing)row;

        _editedPembimbings.Add(new UpdatePembimbingsPembimbing
        {
            PembimbingId = pembimbing.Id,
            Nama = pembimbing.Nama,
            Nip = pembimbing.Nip,
            Jabatan = pembimbing.Jabatan,
        });

        StateHasChanged();
    }

    private void EditPreview(object row)
    {
        var pembimbing = (GetPembimbingsPembimbing)row;

        _pembimbingBeforeEdited = new()
        {
            Nama = pembimbing.Nama,
            Nip = pembimbing.Nip,
            Jabatan = pembimbing.Jabatan,
        };
    }

    private void EditCancel(object row)
    {
        var pembimbing = (GetPembimbingsPembimbing)row;
        pembimbing.Nama = _pembimbingBeforeEdited.Nama;
        pembimbing.Nip = _pembimbingBeforeEdited.Nip;
        pembimbing.Jabatan = _pembimbingBeforeEdited.Jabatan;
    }

    private async Task UpdateEditedApps()
    {
        if (_editedPembimbings.Any())
        {
            var request = new UpdatePembimbingsRequest
            {
                Pembimbings = _editedPembimbings
            };

            var response = await _pembimbingService.UpdatePembimbingsAsync(request);

            if (response.Error is not null)
            {
                _snackbar.AddErrors(response.Error.Details);

                return;
            }

            _snackbar.AddSuccess(SuccessMessageFor.Action($"{response.Result!.AppsUpdated} {DisplayTextFor.Apps}", CommonDisplayTextFor.Updated));

            _editedApps = new List<UpdateAppsApp>();

            await _tableApps.ReloadServerData();
        }
    }

}
