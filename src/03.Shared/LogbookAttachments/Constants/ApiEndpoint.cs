namespace Pertamina.SIMIT.Shared.LogbookAttachments.Constants;
public class ApiEndpoint
{
    public static class V1
    {
        public static class LogbookAttachments
        {
            public const string Segment = $"{nameof(V1)}/{nameof(LogbookAttachments)}";

            public static class RouteTemplateFor
            {
                public const string LogbookAttachmentId = "{logbookAttachmentId:guid}";
                public const string Download = "Download/{logbookAttachmentId:guid}";
            }
        }
    }
}
