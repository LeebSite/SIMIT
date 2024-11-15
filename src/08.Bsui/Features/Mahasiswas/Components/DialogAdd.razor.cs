using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Pembimbings.Queries.GetPembimbingsList;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
public partial class DialogAdd

{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public CreateMahasiswaRequest Request { get; set; } = new();
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

    private async Task HandleValidSubmit()
    {
        _isLoading = true;
        var response = await _mahasiswaService.CreateMahasiswaAsync(Request);
        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;
            return;
        }

        MudDialog.Close(DialogResult.Ok(response.Result?.MahasiswaId));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
