
using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Mahasiswas.Queries.GetMahasiswa;
public class LogbookItem
{
    public Guid LogbookId { get; set; }
    public string Aktifitas { get; set; } = default!;
    public DateTime LogbookDate { get; set; }
}

public class GetMahasiswaResponse : Response
{
    public Guid Id { get; set; }
    public string Nama { get; set; } = default!;
    public string Nim { get; set; } = default!;
    public string Kampus { get; set; } = default!;
    public DateTime MulaiMagang { get; set; } = default!;
    public DateTime SelesaiMagang { get; set; } = default!;
    public string Bagian { get; set; } = default!;
    public Guid PembimbingId { get; set; }
    public string? PembimbingNama { get; set; }

    public Guid LaporanId { get; set; }
    public string? LaporanDeskripsi { get; set; }

    // Tambahkan daftar logbook
    public List<LogbookItem> Logbooks { get; set; } = new();
    public Guid LogbookId { get; set; }
    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;
}

