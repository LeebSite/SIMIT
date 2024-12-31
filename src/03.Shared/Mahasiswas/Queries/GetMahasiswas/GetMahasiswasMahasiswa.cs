using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswas;
public class GetMahasiswasMahasiswa : Response
{
    public Guid Id { get; set; }
    public string Nama { get; set; }
    public string Nim { get; set; } = default!;
    public string Kampus { get; set; } = default!;
    public DateTime MulaiMagang { get; set; } = default!;
    public DateTime SelesaiMagang { get; set; } = default!;
    public string Bagian { get; set; } = default!;
    public Guid PembimbingId { get; set; }

    public int Mahasiswa { get; set; }
    public string? PembimbingNama { get; set; }

    public Guid? LaporanId { get; set; }
    public string? LaporanDeskripsi { get; set; }

    public int TotalMahasiswaAktif { get; set; }
    public int TotalLaporanSubmitted { get; set; }

    public int Laporan { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
