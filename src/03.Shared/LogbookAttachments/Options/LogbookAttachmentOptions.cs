namespace Pertamina.SIMIT.Shared.LogbookAttachments.Options;
public class LogbookAttachmentOptions
{
    public const string SectionKey = nameof(LogbookAttachments);

    public long MaximumFileSizeInBytes { get; set; }
}
