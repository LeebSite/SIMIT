using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbooksList;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
public partial class DialogLogbook
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public List<GetLogbooksList> Logbooks { get; set; } = new List<GetLogbooksList>();

    private bool _calendarInitialized = false;

    private async Task InitializeCalendar()
    {
        if (_calendarInitialized || Logbooks == null)
        {
            return;
        }

        // Konversi tanggal logbook ke array string
        var datesWithLogbook = Logbooks.Select(lb => lb.LogbookDate.ToString("yyyy-MM-dd")).ToArray();

        // Panggil JavaScript untuk inisialisasi kalender
        await _jsRuntime.InvokeVoidAsync("fullCalendar", "calendar", datesWithLogbook);

        _calendarInitialized = true;
    }

    private void CloseDialog()
    {
        MudDialog.Close();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeCalendar();
        }
    }
}
