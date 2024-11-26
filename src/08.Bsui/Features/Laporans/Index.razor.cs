using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Laporans.Commands.CreateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Commands.UpdateLaporan;
using Pertamina.SIMIT.Shared.Laporans.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;

namespace Pertamina.SIMIT.Bsui.Features.Laporans;

public partial class Index
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid MahasiswaId { get; set; }
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private ErrorResponse? _error;
    private bool _isLoading;
    private readonly CreateLaporanModel _model = new();
    public UpdateLaporanRequest Attachment { get; set; } = default!;

    public GetMahasiswaResponse _mahasiswa = default!;

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
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    {
        _model.File = eventArgs.File;
    }
    private async Task OnValidSubmit()
    {
        _error = null;

        if (_model.File is null)
        {
            _snackbar.AddError($"Silakan pilih {CommonDisplayTextFor.File.ToLower()}.");
            return;
        }

        _isLoading = true;

        try
        {
            // Baca file ke MemoryStream
            await using var memoryStream = new MemoryStream();
            await _model.File.OpenReadStream().CopyToAsync(memoryStream);

            // Buat request untuk Create/Update Laporan
            var request = new CreateLaporanRequest
            {
                MahasiswaNim = _model.MahasiswaNim,
                Deskripsi = _model.Deskripsi,
                File = _model.File.ToFormFile(memoryStream, nameof(CreateLaporanRequest.File))
            };

            // Kirim ke layanan
            var response = await _laporanService.CreateLaporanAsync(request);

            if (response.Error is not null)
            {
                // Jika ada error, tampilkan pesan
                _snackbar.AddError($"Gagal menyimpan laporan: {response.Error.Status}");
            }
            else
            {
                // Berhasil
                _snackbar.Add("Laporan berhasil disimpan!");
            }
        }
        catch (Exception ex)
        {
            _snackbar.AddError($"Mahasiswa dengan NIM {_model.MahasiswaNim} tidak ditemukan.");
        }
        //catch (Exception ex)
        //{
        //    _snackbar.AddError($"Terjadi kesalahan saat menyimpan laporan: {ex.Message}");
        //}
        finally
        {
            _isLoading = false;
        }
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        _snackbar.AddErrors(editContext.GetValidationMessages());
    }
}

public class CreateLaporanModel
{
    public Guid LaporanId { get; set; }
    public Guid MahasiswaId { get; set; }
    public string MahasiswaNim { get; set; }
    public string Deskripsi { get; set; }
    public IBrowserFile File { get; set; }
}

public class CreateLaporanModelValidator : AbstractValidator<CreateLaporanModel>
{
    public CreateLaporanModelValidator()
    {
        RuleFor(v => v.MahasiswaNim)
            .NotEmpty();

        RuleFor(v => v.Deskripsi)
            .NotEmpty();

        RuleFor(v => v.File)
            .NotNull();
    }
}
