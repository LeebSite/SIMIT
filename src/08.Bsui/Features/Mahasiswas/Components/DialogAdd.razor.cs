using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
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

    private List<GetPembimbingsList> _pembimbingList = new();
    private bool _isLoading;
    //private ErrorResponse? _error;
    private readonly IList<IBrowserFile> _files = new List<IBrowserFile>();
    private readonly CreateAttachmentMahasiswaModel _model = new();
    private CreateMahasiswaFormModel FormModel { get; set; } = new();

    public class CreateMahasiswaFormModel
    {
        public CreateMahasiswaRequest Request { get; set; } = new();
        public CreateAttachmentMahasiswaModel Attachment { get; set; } = new();
    }

    private void OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    {
        FormModel.Attachment.File = eventArgs.File;
    }

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
        _isLoading = true;

        // Simpan data mahasiswa
        var mahasiswaResponse = await _mahasiswaService.CreateMahasiswaAsync(FormModel.Request);
        if (mahasiswaResponse.Error is not null)
        {
            _snackbar.AddError(mahasiswaResponse.Error.ToString());
            _isLoading = false;
            return;
        }

        if (mahasiswaResponse.Result == null)
        {
            _snackbar.AddError("Gagal mendapatkan data mahasiswa.");
            _isLoading = false;
            return;
        }

        var mahasiswaId = mahasiswaResponse.Result.MahasiswaId;
        _snackbar.AddInfo($"Mahasiswa ID: {mahasiswaId}");

        // Ambil ID Mahasiswa dari response
        //var mahasiswaId = mahasiswaResponse.Result.MahasiswaId;

        // Simpan foto jika ada file yang diunggah
        if (FormModel.Attachment.File is not null)
        {
            await UploadMahasiswaAttachment(mahasiswaId, FormModel.Attachment.File);
        }
        else
        {
            // Tindakan jika file tidak ada
            _snackbar.AddError("File is required.");
        }

        _isLoading = false;
        MudDialog.Close(DialogResult.Ok(mahasiswaId));
    }
    private async Task UploadMahasiswaAttachment(Guid mahasiswaId, IBrowserFile file)
    {
        if (mahasiswaId == Guid.Empty)
        {
            _snackbar.AddError("Invalid mahasiswa ID.");
            return;
        }

        try
        {
            await using var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);

            var attachmentRequest = new CreateMahasiswaAttachmentRequest
            {
                MahasiswaId = mahasiswaId,
                File = file.ToFormFile(memoryStream, nameof(CreateMahasiswaAttachmentRequest.File))
            };

            var attachmentResponse = await _mahasiswaAttachmentService.CreateMahasiswaAttachmentAsync(attachmentRequest);

            if (attachmentResponse.Error is not null)
            {
                _snackbar.AddError(attachmentResponse.Error.ToString());
            }
            else
            {
                _snackbar.AddSuccess("Foto berhasil diunggah.");
            }
        }
        catch (Exception ex)
        {
            _snackbar.AddError($"Terjadi kesalahan saat mengunggah file: {ex.Message}");
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
