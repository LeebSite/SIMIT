using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Mahasiswa : AuditableEntity
{
    public string Nama { get; set; } = default!;
    public string Nim { get; set; } = default!;
    public string Kampus { get; set; } = default!;
    public DateTime MulaiMagang { get; set; } = default!;
    public DateTime SelesaiMagang { get; set; } = default!;
    public string Bagian { get; set; } = default!;

    public Guid PembimbingId { get; set; }
    public Pembimbing Pembimbing { get; set; } = default!;

    // Relasi one-to-many dengan Logbook
    public List<Logbook> Logbooks { get; set; } = new List<Logbook>();

    // Relasi one-to-one dengan Laporan
    public Laporan Laporan { get; set; } = default!;
}
