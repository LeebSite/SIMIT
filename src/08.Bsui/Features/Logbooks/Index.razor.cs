using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Logbooks.Components;
using Pertamina.SIMIT.Bsui.Features.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;

namespace Pertamina.SIMIT.Bsui.Features.Logbooks;

public partial class Index
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetLogbooksLogbook> _tableLogbooks = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;
    //private List<UpdateMahasiswasMahasiswa> _editedMahasiswas = new();
    //private GetLogbooksLogbook _mahasiswaBeforeEdited = new();

    protected override async Task OnInitializedAsync()
    {
        SetupBreadcrumb();
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(DisplayTextFor.Logbooks)
        };

    }
    private async Task<TableData<GetLogbooksLogbook>> ReloadTableLogbooks(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetLogbooksLogbook>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _logbookService.GetLogbooksAsync(request);

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
        await _tableLogbooks.ReloadServerData();
    }

    private async Task ShowDialogAdd()
    {
        var request = new CreateLogbookRequest();

        var parameters = new DialogParameters
        {
            { nameof(DialogAdd.Request), request }
        };

        var dialog = _dialogService.Show<DialogAdd>($"{CommonDisplayTextFor.Add} {DisplayTextFor.Logbook}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var id = (Guid)result.Data;

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Logbook, CommonDisplayTextFor.Created));
            _navigationManager.NavigateTo(RouteFor.Index, forceLoad: true);
        }
    }
}
