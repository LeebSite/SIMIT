using Pertamina.SIMIT.Shared.Common.Extensions;

namespace Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
public class DisplayTextFor
{
    public static readonly string MahasiswaAttachment = nameof(MahasiswaAttachment).SplitWords();

    public const string Attachments = nameof(Attachments);
    public const string Attachment = nameof(Attachment);
}
