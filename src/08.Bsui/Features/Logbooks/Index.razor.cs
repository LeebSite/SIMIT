using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
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
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid MahasiswaId { get; set; }
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetLogbooksLogbook> _tableLogbooks = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;
    private readonly CreateLogbookModel _model = new();
    private bool _isLoading;
    private bool IsMorningSession => DateTime.Now.Hour is >= 7 and <= 12;
    private bool IsAfternoonSession => DateTime.Now.Hour is >= 13 and <= 16;
    private readonly List<GetLogbooksLogbook> _logbooks = new();

    //private bool _useBackCamera = false;
    private string? _capturedPhoto; // Base64 foto
    private bool _isSubmitting = false;
    private bool _isCameraActive = false;

    public CreateLogbookRequest Request { get; set; } = default!;

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
            CommonBreadcrumbFor.Active(DisplayTextFor.LogbookHarianMahasiswa)
        };

    }
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task<TableData<GetLogbooksLogbook>> ReloadTableLogbooks(TableState state)
    {
        _error = null;

        // Memulai proses render ulang
        StateHasChanged();

        var tableData = new TableData<GetLogbooksLogbook>();
        var request = state.ToPaginatedListRequest(_searchKeyword);
        var response = await _logbookService.GetLogbooksAsync(request);

        _error = response.Error;

        if (response.Result is null)
        {
            return tableData;
        }

        // Ambil tanggal hari ini
        var today = DateTime.Today;

        // Filter data hanya untuk hari ini
        var filteredItems = response.Result.Items
            .Where(logbook => logbook.LogbookDate.Date == today)
            .ToList();

        // Gabungkan data berdasarkan NIM
        var groupedItems = filteredItems
     .GroupBy(logbook => logbook.MahasiswaNim)
     .Select(group =>
     {
         var pagiLogbook = group.FirstOrDefault(logbook =>
             logbook.LogbookDate.TimeOfDay >= TimeSpan.FromHours(7) &&
             logbook.LogbookDate.TimeOfDay < TimeSpan.FromHours(12));

         var siangLogbook = group.FirstOrDefault(logbook =>
             logbook.LogbookDate.TimeOfDay >= TimeSpan.FromHours(13) &&
             logbook.LogbookDate.TimeOfDay < TimeSpan.FromHours(16));

         return new GetLogbooksLogbook
         {
             MahasiswaNim = group.Key,
             MahasiswaNama = pagiLogbook?.MahasiswaNama ?? siangLogbook?.MahasiswaNama,
             LogbookDate = (DateTime)(pagiLogbook?.LogbookDate ?? siangLogbook?.LogbookDate),
             Aktifitas = $"{pagiLogbook?.Aktifitas ?? string.Empty} | {siangLogbook?.Aktifitas ?? string.Empty}".Trim(),
             StatusPagi = pagiLogbook != null,  // Status pagi tercentang jika ada logbook pagi
             StatusSiang = siangLogbook != null  // Status siang tercentang jika ada logbook siang
         };
     })
     .ToList();

        // Debug log data setelah filtering
        Console.WriteLine("Filtered data for today:");
        foreach (var item in filteredItems)
        {
            Console.WriteLine($"NIM: {item.MahasiswaNim}, Date: {item.LogbookDate}");
        }

        // Mengembalikan data yang sudah digabung dalam format TableData
        tableData = new TableData<GetLogbooksLogbook>
        {
            Items = groupedItems,
            TotalItems = groupedItems.Count
        };

        // Pastikan UI di-refresh setelah data selesai dimuat
        StateHasChanged();

        return tableData;

    }

    private async Task OnSearch(string keyword)
    {
        _searchKeyword = keyword.Trim();
        await _tableLogbooks.ReloadServerData();
    }

    private async Task StartCamera()
    {
        _isCameraActive = true;
        await _jsRuntime.InvokeVoidAsync("startCamera", "video");
        StateHasChanged();
    }

    //private async Task SwitchCamera()
    //{
    //    _useBackCamera = !_useBackCamera;
    //    await StartCamera();
    //}

    private void ResetPhoto()
    {
        _capturedPhoto = null;
        _isCameraActive = true; // Re-activate the camera
        StateHasChanged();
    }

    private async Task CapturePhoto()
    {
        try
        {
            // Gunakan await untuk memastikan bahwa foto hanya diambil setelah video siap
            var photoData = await _jsRuntime.InvokeAsync<string>("capturePhoto", "video", "canvas");

            // Debug: log panjang data untuk memastikan ukuran yang diterima
            Console.WriteLine($"Captured photo length: {photoData?.Length ?? 0}");

            if (string.IsNullOrEmpty(photoData))
            {
                Console.Error.WriteLine("Failed to capture photo.");
                return;
            }

            _capturedPhoto = photoData;
            _isCameraActive = false; // Deactivate the camera after photo capture
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error capturing photo: {ex.Message}");
        }
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
    private async Task OnValidSubmit()
    {
        _error = null;
        _isLoading = true;
        _isSubmitting = true;

        var base64Data = _capturedPhoto?.Split(',')[1];
        var imageBytes = Convert.FromBase64String(base64Data);

        var fileName = $"{Guid.NewGuid()}.jpeg";
        using var memoryStream = new MemoryStream(imageBytes);
        var formFile = new FormFile(memoryStream, 0, imageBytes.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"  // Correct content type for JPEG
        };
        var geolocationResult = await _geolocationService.GetCurrentPosition();
        var coordinates = geolocationResult.Position.Coords;
        var request = new CreateLogbookRequest
        {
            MahasiswaNim = _model.MahasiswaNim,
            LogbookDate = _model.LogbookDate,
            Aktifitas = _model.Aktifitas,
            File = formFile,
            Latitude = coordinates.Latitude,
            Longitude = coordinates.Longitude,
            Accuracy = coordinates.Accuracy
        };

        var response = await _logbookService.CreateLogbookAsync(request);

        if (response.Error != null)
        {
            _error = response.Error;
        }
        else
        {
            _snackbar.Add("Logbook berhasil ditambahkan.");
        }

        _isLoading = false;
    }

    //private async Task OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    //{
    //    var browserFile = eventArgs.File;

    //    if (browserFile is not null)
    //    {
    //        // Simpan stream dari IBrowserFile
    //        var stream = browserFile.OpenReadStream(browserFile.Size);

    //        // Bungkus IBrowserFile ke IFormFile menggunakan MemoryStream
    //        var memoryStream = new MemoryStream();
    //        await stream.CopyToAsync(memoryStream);
    //        memoryStream.Position = 0; // Reset posisi stream agar bisa dibaca ulang

    //        Request.File = new FormFile(memoryStream, 0, memoryStream.Length, browserFile.Name, browserFile.Name)
    //        {
    //            Headers = new HeaderDictionary(), // Optional
    //            ContentType = browserFile.ContentType
    //        };
    //    }
    //}

    private void OnInvalidSubmit(EditContext editContext)
    {
        _snackbar.AddErrors(editContext.GetValidationMessages());
    }
    //private bool IsLogbookSubmitted(string mahasiswaNim, string period)
    //{
    //    var (pagiStart, pagiEnd) = (new TimeSpan(7, 0, 0), new TimeSpan(12, 0, 0));
    //    var (siangStart, siangEnd) = (new TimeSpan(12, 1, 0), new TimeSpan(16, 0, 0));

    //    return _logbooks.Any(log =>
    //        log.MahasiswaNim == mahasiswaNim &&
    //        log.LogbookDate.Date == DateTime.Today &&
    //        (
    //            (period == "Pagi" && log.LogbookDate.TimeOfDay >= pagiStart && log.LogbookDate.TimeOfDay <= pagiEnd) ||
    //            (period == "Siang" && log.LogbookDate.TimeOfDay >= siangStart && log.LogbookDate.TimeOfDay <= siangEnd)
    //        ));
    //}

}

public class CreateLogbookModel
{
    public Guid LogbookId { get; set; }
    public Guid MahasiswaId { get; set; }
    public string MahasiswaNim { get; set; }
    public string Session => LogbookDate.Hour is >= 7 and <= 12 ? "Pagi" :
                            LogbookDate.Hour is >= 13 and <= 16 ? "Siang" : "Tidak Dikenal";
    public DateTime LogbookDate { get; set; } = DateTime.Now;
    public string Aktifitas { get; set; }
    public IBrowserFile File { get; set; }
}

public class CreateLogbookModelValidator : AbstractValidator<CreateLogbookModel>
{
    public CreateLogbookModelValidator()
    {
        RuleFor(v => v.MahasiswaNim)
            .NotEmpty();

        RuleFor(v => v.Aktifitas)
            .NotEmpty();

        RuleFor(v => v.File)
            ;
    }
}
