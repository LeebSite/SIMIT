
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Components;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas;
public partial class Details
{
    [Parameter]
    public Guid MahasiswaId { get; set; }

    private bool _isLoading;
    private ErrorResponse? _error;

    //private string? _errorMessage;

    private List<BreadcrumbItem> _breadcrumbItems = new();
    private GetMahasiswaResponse _mahasiswa = default!;
    private string? _imageData;

    protected override async Task OnParametersSetAsync()
    {
        await Reload();
    }
    private async Task Reload()
    {
        SetupBreadcrumb();

        await GetMahasiswa();

        if (_mahasiswa is null)
        {
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Error));

            return;
        }

        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_mahasiswa.Nim));
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            BreadcrumbFor.Index
        };
    }
    private void NavigateToLogbook()
    {
        _navigationManager.NavigateTo(@RouteFor.Logbooks(MahasiswaId));
    }

    private void NavigateToLogbookDetails()
    {
        _navigationManager.NavigateTo(@RouteFor.LogbooksDetail(MahasiswaId));
    }

    //private async Task ShowLogbookDialog()
    //{
    //    var logbooks = _mahasiswa.Logbooks.Select(lb => new GetLogbooksList
    //    {
    //        LogbookDate = lb.LogbookDate,
    //    }).ToList();

    //    var dialog = _dialogService.Show<DialogLogbook>("Logbook", new DialogParameters
    //{
    //    { "Logbooks", logbooks }
    //});

    //    var result = await dialog.Result;
    //}

    private async Task DownloadFoto()
    {
        //_isLoading = true;

        //var response = await _mahasiswaAttachmentService.GetMahasiswaAttachmentFileAsync(Attachment.Id);

        //_isLoading = false;

        //if (response.Error is not null)
        //{
        //    _error = response.Error;

        //    return;
        //}

        //await _jsRuntime.InvokeVoidAsync(
        //    JavaScriptIdentifierFor.DownloadFile,
        //    response.Result!.FileName,
        //    response.Result.ContentType,
        //    response.Result.Content);

        Console.WriteLine($"MahasiswaId: {_mahasiswa.MahasiswaAttachmentId}");

        _isLoading = true;
        if (_mahasiswa.MahasiswaAttachmentId == null)
        {
            _snackbar.Add("Mahasiswa ini tidak memiliki laporan untuk diunduh.", Severity.Warning);
            return;
        }
        // Panggil API untuk mendapatkan laporan
        var response = await _mahasiswaAttachmentService.GetMahasiswaAttachmentFileAsync(_mahasiswa.MahasiswaAttachmentId);

        _isLoading = false;

        // Periksa apakah ada error dalam respons API
        if (response.Error is not null)
        {
            _error = response.Error;
            return;
        }

        // Jalankan fungsi JavaScript untuk mengunduh file
        await _jsRuntime.InvokeVoidAsync(
            JavaScriptIdentifierFor.DownloadFile,
            response.Result!.FileName,
            response.Result.ContentType,
            response.Result.Content);
    }

    private async Task Download()
    {
        Console.WriteLine($"LaporanId: {_mahasiswa.LaporanId}");

        _isLoading = true;
        if (_mahasiswa.LaporanId == null)
        {
            _snackbar.Add("Mahasiswa ini tidak memiliki laporan untuk diunduh.", Severity.Warning);
            return;
        }
        // Panggil API untuk mendapatkan laporan
        var response = await _laporanService.GetLaporanAsync(_mahasiswa.LaporanId);

        _isLoading = false;

        // Periksa apakah ada error dalam respons API
        if (response.Error is not null)
        {
            _error = response.Error;
            return;
        }

        // Jalankan fungsi JavaScript untuk mengunduh file
        await _jsRuntime.InvokeVoidAsync(
            JavaScriptIdentifierFor.DownloadFile,
            response.Result!.FileName,
            response.Result.ContentType,
            response.Result.Content);
    }

    private async Task GetMahasiswa()
    {
        _isLoading = true;
        _error = null;

        try
        {
            var response = await _mahasiswaService.GetMahasiswaAsync(MahasiswaId);
            _isLoading = false;

            if (response.Error != null)
            {
                _error = response.Error;
                return;
            }

            _mahasiswa = response.Result ?? new GetMahasiswaResponse(); // Menghindari null
            _imageData = $"data:image/jpeg;base64,{Convert.ToBase64String(response.Result!.Content)}";

        }
        catch (Exception ex)
        {
            _isLoading = false;
        }

    }

    private async Task ShowDialogEdit()
    {
        var request = new UpdateMahasiswaRequest
        {
            MahasiswaId = _mahasiswa.Id,
            Nama = _mahasiswa.Nama,
            Nim = _mahasiswa.Nim,
            Kampus = _mahasiswa.Kampus,
            MulaiMagang = _mahasiswa?.MulaiMagang,
            SelesaiMagang = _mahasiswa?.SelesaiMagang,
            Bagian = _mahasiswa.Bagian,
            PembimbingId = _mahasiswa.PembimbingId,
        };

        var parameters = new DialogParameters
        {
            { nameof(DialogEdit.Request), request }
        };

        var dialog = _dialogService.Show<DialogEdit>($"{CommonDisplayTextFor.Edit} {DisplayTextFor.Mahasiswa}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Mahasiswa, _mahasiswa.Nama, CommonDisplayTextFor.Updated));

            await Reload();
        }
    }

    private async Task ShowDialogDelete()
    {
        var dialog = _dialogService.Show<DialogDelete>($"{CommonDisplayTextFor.Delete} {DisplayTextFor.Mahasiswa} {_mahasiswa.Nama}");
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var response = await _mahasiswaService.DeleteMahasiswaAsync(_mahasiswa.Id);

            if (response.Error is not null)
            {
                _snackbar.AddErrors(response.Error.Details);

                return;
            }

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Mahasiswa, _mahasiswa.Nama, CommonDisplayTextFor.Deleted));
            _navigationManager.NavigateTo(RouteFor.Index);
        }
    }
}

