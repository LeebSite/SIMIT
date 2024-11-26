namespace Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporans;
public class GetLaporansLaporan
{
    public Guid Id { get; set; }

    public string Deskripsi { get; set; } = default!;

    public Guid MahasiswaId { get; set; }
    public string MahasiswaNama { get; set; }
    public string MahasiswaNim { get; set; }
    public string MahasiswaKampus { get; set; }
}
