using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.LogbookAttachments.Constants;
public class DisplayTextFor
{
    public static readonly string LogbookAttachment = nameof(LogbookAttachment).SplitWords();

    public const string Attachments = nameof(Attachments);
    public const string Attachment = nameof(Attachment);
}
