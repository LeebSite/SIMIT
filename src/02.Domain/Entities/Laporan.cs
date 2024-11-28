using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Laporan : FileEntity
{
    public string Deskripsi { get; set; } = default!;

    // Foreign key ke Mahasiswa untuk relasi one-to-one
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;
}
