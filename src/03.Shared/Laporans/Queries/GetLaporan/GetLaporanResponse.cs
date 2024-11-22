using Pertamina.SIMIT.Shared.Common.Responses;

namespace Pertamina.SIMIT.Shared.Laporans.Queries.GetLaporan;
public class GetLaporanResponse : Response
{
    public Guid Id { get; set; }
    public string FileLaporan { get; set; } = default!;
    public string FileProject { get; set; } = default!;
    public string Deskripsi { get; set; } = default!;
    public Guid MahasiswaId { get; set; }
    public string? MahasiswaName { get; set; }

    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
