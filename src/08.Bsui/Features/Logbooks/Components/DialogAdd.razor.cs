using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Logbooks.Commands.CreateLogbook;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswasList;

namespace Pertamina.SIMIT.Bsui.Features.Logbooks.Components;

public partial class DialogAdd
{

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public CreateLogbookRequest Request { get; set; } = default!;
    private List<GetMahasiswasList> _mahasiswaList = new();
    private bool _isLoading;
    private ErrorResponse? _error;

    protected override async Task OnInitializedAsync()
    {
        var response = await _mahasiswaService.GetMahasiswasListAsync();
        if (response.Error is null)
        {
            _mahasiswaList = response.Result!.Items.ToList();
        }

    }

    private async Task OnValidSubmit()
    {
        _error = null;

        _isLoading = true;
        var response = await _logbookService.CreateLogbookAsync(Request);
        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        MudDialog.Close(DialogResult.Ok(response.Result!.LogbookId));
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
