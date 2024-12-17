namespace Pertamina.SIMIT.Shared.MahasiswaAttachments.Constants;
public class ApiEndpoint
{
    public static class V1
    {
        public static class MahasiswaAttachments
        {
            public const string Segment = $"{nameof(V1)}/{nameof(MahasiswaAttachments)}";

            public static class RouteTemplateFor
            {
                public const string MahasiswaAttachmentId = "{mahasiswaAttachmentId:guid}";
                public const string Download = "Downloads/{mahasiswaAttachmentId:guid}";
            }
        }
    }
}
