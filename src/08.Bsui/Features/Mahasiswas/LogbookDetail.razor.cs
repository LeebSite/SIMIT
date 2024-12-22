using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas;
public partial class LogbookDetail
{
    [Parameter]
    public Guid MahasiswaId { get; set; }

    private MudTable<GetLogbooksLogbook> _tableLogbooks = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;

    private async Task<TableData<GetLogbooksLogbook>> ReloadTableLogbooks(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetLogbooksLogbook>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _logbookService.GetLogbooksByIdAsync(request, MahasiswaId);

        _error = response.Error;

        if (response.Result is null)
        {
            return tableData;
        }

        // Menyesuaikan hasil untuk file gambar atau lainnya
        foreach (var logbook in response.Result.Items)
        {
            if (logbook.Content == null || string.IsNullOrEmpty(logbook.ContentType))
            {
                Console.WriteLine($"Logbook ID: {logbook.LogbookId} has invalid content or contentType.");
            }
        }

        StateHasChanged();

        return response.Result.ToTableData();
    }

    private async Task OnSearch(string keyword)
    {
        _searchKeyword = keyword.Trim();

        await _tableLogbooks.ReloadServerData();
    }
}
