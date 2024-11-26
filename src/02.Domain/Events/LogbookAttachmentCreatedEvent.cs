namespace Pertamina.SIMIT.Domain.Events;
public class LogbookAttachmentCreatedEvent : DomainEvent
{
    public Guid LogbookAttachmentId { get; set; }

    public LogbookAttachmentCreatedEvent(Guid logbookAttachmentId)
    {
        LogbookAttachmentId = logbookAttachmentId;
    }
}
