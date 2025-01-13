using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.MahasiswaAttachments.Commands.CreateMahasiswaAttachment;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbingsList;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
public partial class DialogAdd

{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid MahasiswaId { get; set; }

    [Parameter]
    public CreateMahasiswaRequest Request { get; set; } = default!;
    //private readonly CreateAttachmentMahasiswaModel _model = new();
    private List<GetPembimbingsList> _pembimbingList = new();
    private bool _isLoading;
    private ErrorResponse? _error;

    protected override async Task OnInitializedAsync()
    {
        var response = await _pembimbingService.GetPembimbingsListAsync();
        if (response.Error is null)
        {
            _pembimbingList = response.Result!.Items.ToList();
        }

    }

    private async Task OnValidSubmit()
    {
        _error = null;
        _isLoading = true;

        try
        {
            // Kirim request untuk membuat mahasiswa
            var createMahasiswaResponse = await _mahasiswaService.CreateMahasiswaAsync(Request);

            if (createMahasiswaResponse.Error != null)
            {
                _error = createMahasiswaResponse.Error;
                return;
            }

            // Ambil ID mahasiswa yang baru saja dibuat
            var mahasiswaId = createMahasiswaResponse.Result!.MahasiswaId;

            // Buat request untuk *attachment* menggunakan ID mahasiswa
            var attachmentRequest = new CreateMahasiswaAttachmentRequest
            {
                MahasiswaId = mahasiswaId,
                File = Request.File
            };

            // Kirim request untuk menyimpan *attachment*
            var createAttachmentResponse = await _mahasiswaAttachmentService.CreateMahasiswaAttachmentAsync(attachmentRequest);

            if (createAttachmentResponse.Error != null)
            {
                _error = createAttachmentResponse.Error;
                return;
            }

            _snackbar.AddSuccess($"Mahasiswa dengan NIM {Request.Nim} berhasil ditambahkan.");

            // Tutup dialog jika berhasil
            MudDialog.Close(DialogResult.Ok(mahasiswaId));
            //_navigationManager.NavigateTo(RouteFor.Index, forceLoad: true);
        }
        catch (Exception ex)
        {
            //_error = new ErrorResponse { Message = ex.Message };
        }
        finally
        {
            _navigationManager.NavigateTo(RouteFor.Index, forceLoad: true);
            _isLoading = false;
        }
    }

    private async Task OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    {
        var browserFile = eventArgs.File;

        if (browserFile is not null)
        {
            // Simpan stream dari IBrowserFile
            var stream = browserFile.OpenReadStream(browserFile.Size);

            // Bungkus IBrowserFile ke IFormFile menggunakan MemoryStream
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset posisi stream agar bisa dibaca ulang

            Request.File = new FormFile(memoryStream, 0, memoryStream.Length, browserFile.Name, browserFile.Name)
            {
                Headers = new HeaderDictionary(), // Optional
                ContentType = browserFile.ContentType
            };
        }
    }

    public class CreateAttachmentMahasiswaModel
    {
        public IBrowserFile? File { get; set; }
    }

    public class CreateAttachmentMahasiswaModelValidator : AbstractValidator<CreateAttachmentMahasiswaModel>
    {
        public CreateAttachmentMahasiswaModelValidator()
        {

            RuleFor(v => v.File)
                .NotNull();
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
    private void OnInvalidSubmit(EditContext editContext)
    {
        _snackbar.AddErrors(editContext.GetValidationMessages());
    }
}
