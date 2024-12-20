using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas;
public partial class Logbook
{
    [Parameter]
    public Guid MahasiswaId { get; set; }

    private List<string> _logbookDates = new();
    private bool _isLoading = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var response = await _mahasiswaService.GetMahasiswaAsync(MahasiswaId);

                // If valid response, process the logbook dates
                if (response.Result?.Logbooks?.Any() == true)
                {
                    _logbookDates = response.Result.Logbooks
                        .Where(lb => lb.LogbookDate != null) // Ensure valid date
                        .Select(lb => lb.LogbookDate.ToString("yyyy-MM-dd"))
                        .ToList();
                }

                // Always invoke JavaScript to initialize the calendar
                await _jsRuntime.InvokeVoidAsync("initializeCalendar", "#calendar", _logbookDates.ToArray());
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the process
                Console.WriteLine($"Error loading logbook dates: {ex.Message}");
            }
            finally
            {
                // Ensure loading state is updated
                _isLoading = false;
            }
        }
    }
}
