using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Features.Logbooks.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Queries.GetLogbook;

namespace Pertamina.SIMIT.Bsui.Features.Logbooks;

public partial class Details
{
    [Parameter]
    public Guid LogbookId { get; set; }

    private bool _isLoading;
    private readonly ErrorResponse? _error;
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private readonly List<GetLogbookResponse> _logbooks = new(); // Daftar logbook
    private GetLogbookResponse _logbook = default!;
    private bool _isDisposed;

    protected override async Task OnParametersSetAsync()
    {
        await Reload();
    }
    private async Task Reload()
    {
        SetupBreadcrumb();

        await GetLogbook();

        if (_logbook is null)
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));

            return;
        }

        // Menyaring logbook berdasarkan id mahasiswa
        var filteredLogbook = _logbooks.Where(l => l.MahasiswaNim == LogbookId.ToString()).ToList();

        if (filteredLogbook.Any())
        {
            _logbook = filteredLogbook.First(); // Pilih logbook pertama jika ada yang cocok
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_logbook.MahasiswaNim));
        }
        else
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));
        }
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            BreadcrumbFor.Index
        };
    }

    private async Task GetLogbook()
    {
        _isLoading = true;
        if (_isDisposed)
        {
            return; // Cek apakah komponen sudah di-dispose
        }

        _isLoading = true;
        try
        {
            var response = await _logbookService.GetLogbookAsync(LogbookId);
            if (_isDisposed)
            {
                return; // Cek lagi setelah operasi selesai
            }

            // Proses data jika masih valid
            _logbook = response.Result ?? new GetLogbookResponse();
        }
        catch (Exception ex)
        {
            _isLoading = false;
        }
    }
    // Metode untuk menandakan bahwa komponen telah di-dispose
    public void Dispose()
    {

        _isDisposed = true;
    }

}
