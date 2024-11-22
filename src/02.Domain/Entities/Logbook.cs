using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Logbook : AuditableEntity
{
    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;

    // Foreign key ke Mahasiswa
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;

}
