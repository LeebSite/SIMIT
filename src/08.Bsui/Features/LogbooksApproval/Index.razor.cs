using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.GetLogbook;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooks;

namespace Pertamina.SIMIT.Bsui.Features.LogbooksApproval;
public partial class Index
{
    [Parameter]
    public Guid MahasiswaId { get; set; }

    private MudTable<GetLogbooksLogbook> _tableLogbooks = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    private ClaimsPrincipal? _user;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        _user = authState.User;
    }

    private async Task<TableData<GetLogbooksLogbook>> ReloadTableLogbooks(TableState state)
    {
        _error = null;
        StateHasChanged();

        var tableData = new TableData<GetLogbooksLogbook>();
        //var userEmail = _user?.GetUsername() ?? string.Empty;
        var userEmail = "test@gmail.com";

        var request = new GetLogbookRequest
        {
            Page = state.Page + 1,
            PageSize = state.PageSize,
            SearchText = _searchKeyword,
            SortField = state.SortLabel,
            SortOrder = (SortOrder)state.SortDirection,
            User = userEmail
        };

        var response = await _logbookService.GetLogbooksApprovalAsync(request);

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
