using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class MahasiswaAttachment : FileEntity
{
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;

    //public IList<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
