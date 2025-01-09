using Pertamina.SIMIT.Base.ValueObjects;
using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class Logbook : AuditableEntity
{

    public DateTime LogbookDate { get; set; } = default!;
    public string Aktifitas { get; set; } = default!;
    public Geolocation? FromGeolocation { get; set; }

    public bool Approval { get; set; }

    // Foreign key ke Mahasiswa
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;
    public IList<LogbookAttachment> Attachments { get; set; } = new List<LogbookAttachment>();

}
