using MudBlazor;
using Pertamina.SIMIT.Bsui.Common.Constants;
using Pertamina.SIMIT.Bsui.Common.Extensions;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Components;
using Pertamina.SIMIT.Bsui.Features.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Common.Constants;
using Pertamina.SIMIT.Shared.Common.Enums;
using Pertamina.SIMIT.Shared.Common.Responses;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.CreateMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.GetMahasiswa;
using Pertamina.SIMIT.Shared.Mahasiswas.Commands.UpdateMahasiswas;
using Pertamina.SIMIT.Shared.Mahasiswas.Constants;
using Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;

namespace Pertamina.SIMIT.Bsui.Features.Mahasiswas;
public partial class Index
{
    private List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetMahasiswasMahasiswa> _tableMahasiswas = new();
    private string? _searchKeyword;
    private ErrorResponse? _error;
    private List<UpdateMahasiswasMahasiswa> _editedMahasiswas = new();
    private GetMahasiswasMahasiswa _mahasiswaBeforeEdited = new();
    //private FilterMahasiswa _requestMahasiswa = new();

    protected override async Task OnInitializedAsync()
    {
        SetupBreadcrumb();
    }

    private void SetupBreadcrumb()
    {
        _breadcrumbItems = new()
        {
            CommonBreadcrumbFor.Home,
            CommonBreadcrumbFor.Active(DisplayTextFor.DataMahasiswa)
        };

    }
    private async Task<TableData<GetMahasiswasMahasiswa>> ReloadTableMahasiswas(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetMahasiswasMahasiswa>();
        //var request = state.ToPaginatedListRequest(_searchKeyword);

        var request = new GetMahasiswaRequest
        {
            Page = state.Page + 1,
            PageSize = state.PageSize,
            SearchText = _searchKeyword,
            SortField = state.SortLabel,
            SortOrder = (SortOrder)state.SortDirection,
            //Kampus = _requestMahasiswa.Kampus,
            //Bagian = _requestMahasiswa.Bagian,
        };

        var response = await _mahasiswaService.GetMahasiswasAsync(request);

        _error = response.Error;

        if (response.Result is null)
        {
            return tableData;
        }

        StateHasChanged();

        return response.Result.ToTableData();
    }

    private async Task OnSearch(string keyword)
    {
        _searchKeyword = keyword.Trim();
        await _tableMahasiswas.ReloadServerData();
    }

    private async Task ShowDialogAdd()
    {
        var request = new CreateMahasiswaRequest();

        var parameters = new DialogParameters
        {
            { nameof(DialogAdd.Request), request }
        };

        var dialog = _dialogService.Show<DialogAdd>($"{CommonDisplayTextFor.Add} {DisplayTextFor.Mahasiswa}", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var id = (Guid)result.Data;

            _snackbar.AddSuccess(SuccessMessageFor.Action(DisplayTextFor.Mahasiswa, CommonDisplayTextFor.Created));
            _navigationManager.NavigateTo(RouteFor.Index, forceLoad: true);
        }
    }
    private void EditCommit(object row)
    {
        var mahasiswa = (GetMahasiswasMahasiswa)row;

        _editedMahasiswas.Add(new UpdateMahasiswasMahasiswa
        {
            MahasiswaId = mahasiswa.Id,
            Nama = mahasiswa.Nama,
            Nim = mahasiswa.Nim,
            Kampus = mahasiswa.Kampus,
            MulaiMagang = mahasiswa.MulaiMagang,
            SelesaiMagang = mahasiswa.SelesaiMagang,
            Bagian = mahasiswa.Bagian,
            PembimbingId = mahasiswa.PembimbingId
        });

        StateHasChanged();
    }

    private void EditPreview(object row)
    {
        var mahasiswa = (GetMahasiswasMahasiswa)row;

        _mahasiswaBeforeEdited = new()
        {
            Nama = mahasiswa.Nama,
            Nim = mahasiswa.Nim,
            Kampus = mahasiswa.Kampus,
            MulaiMagang = mahasiswa.MulaiMagang,
            SelesaiMagang = mahasiswa.SelesaiMagang,
            Bagian = mahasiswa.Bagian,
            PembimbingId = mahasiswa.PembimbingId
        };
    }

    private void EditCancel(object row)
    {
        var mahasiswa = (GetMahasiswasMahasiswa)row;
        mahasiswa.Nama = _mahasiswaBeforeEdited.Nama;
        mahasiswa.Nim = _mahasiswaBeforeEdited.Nim;
        mahasiswa.Kampus = _mahasiswaBeforeEdited.Kampus;
        mahasiswa.SelesaiMagang = _mahasiswaBeforeEdited.SelesaiMagang;
        mahasiswa.MulaiMagang = _mahasiswaBeforeEdited.MulaiMagang;
        mahasiswa.Bagian = _mahasiswaBeforeEdited.Bagian;
        mahasiswa.PembimbingId = _mahasiswaBeforeEdited.PembimbingId;

    }

    private async Task UpdateEditedMahasiswas()
    {
        if (_editedMahasiswas.Any())
        {
            var request = new UpdateMahasiswasRequest
            {
                Mahasiswas = _editedMahasiswas
            };

            var response = await _mahasiswaService.UpdateMahasiswasAsync(request);

            if (response.Error is not null)
            {
                _snackbar.AddErrors(response.Error.Details);

                return;
            }

            _snackbar.AddSuccess(SuccessMessageFor.Action($"{response.Result!.MahasiswasUpdated} {DisplayTextFor.Mahasiswas}", CommonDisplayTextFor.Updated));

            _editedMahasiswas = new List<UpdateMahasiswasMahasiswa>();

            await _tableMahasiswas.ReloadServerData();
        }
    }

    private class FilterMahasiswa
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchText { get; set; }
        public string? SortField { get; set; }
        public SortOrder? SortOrder { get; set; }
        public string Kampus { get; set; } = default!;
        public string Bagian { get; set; } = default!;
    }
}
