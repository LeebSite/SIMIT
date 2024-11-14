using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Laporan : AuditableEntity
{
    public string FileLaporan { get; set; } = default!;
    public string FileProject { get; set; } = default!;
    public string Deskripsi { get; set; } = default!;

    // Foreign key ke Mahasiswa untuk relasi one-to-one
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;
}
