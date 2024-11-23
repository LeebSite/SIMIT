using Pertamina.SIMIT.Domain.Abstracts;
using Pertamina.SIMIT.Domain.Events;

namespace Pertamina.SIMIT.Domain.Entities;
public class MahasiswaAttachment : FileEntity, IHasDomainEvent
{
    public Guid MahasiswaId { get; set; }
    public Mahasiswa Mahasiswa { get; set; } = default!;

    public IList<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
