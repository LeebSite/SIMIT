using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;
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
    private readonly CreateAttachmentMahasiswaModel _model = new();
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
        var response = await _mahasiswaService.CreateMahasiswaAsync(Request);
        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        MudDialog.Close(DialogResult.Ok((object)response.Result!.MahasiswaId));

    }

    private void OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    {
        _model.File = eventArgs.File;
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
