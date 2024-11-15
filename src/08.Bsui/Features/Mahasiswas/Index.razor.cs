using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas;
public partial class Index
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetMahasiswasMahasiswa> _tableMahasiswas = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new List<BreadcrumbItem>
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(DisplayTextFor.Mahasiswas ?? "Mahasiswas")
        };

    }
    private async Task<TableData<GetMahasiswasMahasiswa>> ReloadTableApps(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetMahasiswasMahasiswa>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _mahasiswaService.GetMahasiswasAsync(request);

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
        await _tableMahasiswas.ReloadServerData();
    }

    private async Task ShowDialogAdd()
    {
        var request = new CreateMahasiswaRequest();

        var parameters = new DialogParameters
        {
            { nameof(DialogAdd.Request), request }
        };

        var dialog = _dialogService.Show<DialogAdd>($"{CommonDisplayTextFor.Add} {DisplayTextFor.Mahasiswa}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var id = (Guid)result.Data;

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Mahasiswa, CommonDisplayTextFor.Created));
            _navigationManager.NavigateTo(RouteFor.Details(id));
        }
    }
}
