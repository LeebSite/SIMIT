namespace Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;
public class GetLaporansLaporan
{
    public Guid Id { get; set; }
    public string FileLaporan { get; set; } = default!;
    public string FileProject { get; set; } = default!;
    public string Deskripsi { get; set; } = default!;
    public Guid MahasiswaId { get; set; }
    public string? MahasiswaNama { get; set; }
    public string? MahasiswaKampus { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
