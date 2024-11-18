using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Pembimbings.Commands.UpdatePembimbing;

namespace Pertamina.SIMIT.Bsui.Features.Pembimbings.Components;
public partial class DialogEdit
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default;

    [Parameter]
    public UpdatePembimbingRequest Request { get; set; } = default!;

    private bool _isLoading;
    private ErrorResponse? _error;

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task OnValidSubmit()
    {
        _error = null;

        _isLoading = true;

        var response = await _pembimbingService.UpdatePembimbingAsync(Request);

       _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;

        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        _snackbar.AddErrors(editContext.GetValidationMessages());
    }
}
