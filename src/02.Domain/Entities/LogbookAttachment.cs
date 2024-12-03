using Pertamina.SIMIT.Domain.Abstracts;

namespace Pertamina.SIMIT.Domain.Entities;
public class LogbookAttachment : FileEntity
{
    public Guid LogbookId { get; set; }
    public Logbook Logbook { get; set; }
}

