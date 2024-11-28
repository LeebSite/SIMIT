using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    private bool IsAuthorized => true; // Ubah logika otorisasi untuk admin
    private bool IsMorningSession => DateTime.Now.Hour is >= 7 and <= 12;
    private bool IsAfternoonSession => DateTime.Now.Hour is >= 13 and <= 16;
    private readonly List<GetLogbooksLogbook> _logbooks = new();

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
            CommonBreadcrumbFor.Active(DisplayTextFor.Logbooks)
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

        // Debug log data setelah filtering
        Console.WriteLine("Filtered data for today:");
        foreach (var item in filteredItems)
        {
            Console.WriteLine($"NIM: {item.MahasiswaNim}, Date: {item.LogbookDate}");
        }

        // Mengembalikan data yang sudah difilter dalam format TableData
        tableData = new TableData<GetLogbooksLogbook>
        {
            Items = filteredItems,
            TotalItems = filteredItems.Count
        };

        // Pastikan UI di-refresh setelah data selesai dimuat
        StateHasChanged();

        return tableData;

    }

    //private void OnAttachmentFileSelected(InputFileChangeEventArgs eventArgs)
    //{
    //    _model.File = eventArgs.File;
    //}

    private async Task OnSearch(string keyword)
    {
        _searchKeyword = keyword.Trim();
        await _tableLogbooks.ReloadServerData();
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

        var request = new CreateLogbookRequest
        {
            MahasiswaNim = _model.MahasiswaNim,
            LogbookDate = _model.LogbookDate,
            Aktifitas = _model.Aktifitas,
        };

        var response = await _logbookService.CreateLogbookAsync(request);

        _isLoading = false;

        if (response.Error is not null)
        {
            _error = response.Error;

            return;
        }

        _snackbar.Add("Logbook Berhasil ditambahkan");

    }
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
}

public class CreateLogbookModelValidator : AbstractValidator<CreateLogbookModel>
{
    public CreateLogbookModelValidator()
    {
        RuleFor(v => v.MahasiswaNim)
            .NotEmpty();

        RuleFor(v => v.Aktifitas)
            .NotEmpty();

        //RuleFor(v => v.File)
        //    .NotNull();
    }
}

