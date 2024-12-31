using Microsoft.JSInterop;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Bsui.Features.Dashboards;
public partial class Index
{
    private GetMahasiswasMahasiswa _mahasiswa = default!;
    private Dictionary<string, int>? _data;
    private bool _isChartRendered = false;

    protected override async Task OnParametersSetAsync()
    {
        var response = await _mahasiswaService.GetMahasiswaCountAsync();

        StateHasChanged();
        _mahasiswa = response.Result ?? new GetMahasiswasMahasiswa();
    }

    protected override async Task OnInitializedAsync()
    {
        // This is for initial data loading
        _data = await _logbookService.GetLogbooksCountAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isChartRendered && _data != null)
        {
            await Task.Delay(100); // Delay opsional untuk memastikan DOM selesai dirender
            var labels = _data.Keys.ToList();
            var values = _data.Values.ToList();

            Console.WriteLine("Calling renderBarChart...");
            await _jsRuntime.InvokeVoidAsync("renderBarChart", labels, values);

            _isChartRendered = true;
        }
    }

}
