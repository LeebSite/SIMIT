using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;

namespace Pertamina.SIMIT.Bsui.Features.Laporans;
public partial class Index
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private ErrorResponse? _error;
    private string? _searchKeyword;
    private MudTable<GetLaporansLaporan> _tableLaporans = new();

    protected override async Task OnInitializedAsync()
    {
        SetupBreadcrumb();
    }
    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(DisplayTextFor.DokumenMahasiswa)
        };

    }
    private async Task<TableData<GetLaporansLaporan>> ReloadTableLaporans(TableState state)
    {
        _error = null;
        StateHasChanged();

        var tableData = new TableData<GetLaporansLaporan>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _laporanService.GetLaporansAsync(request);

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
        await _tableLaporans.ReloadServerData();
    }
}
