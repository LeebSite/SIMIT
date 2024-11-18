using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Components;
using Pertamina.SIMIT.Bsui.Features.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.CreatePembimbing;
using Pertamina.SIMIT.Shared.Pembimbings.Constants;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbings;

namespace Pertamina.SIMIT.Bsui.Features.Pembimbings;
public partial class Index
{
    private ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetPembimbingsPembimbing> _tablePembimbings = new();
    private string? _searchKeyword;
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

}
