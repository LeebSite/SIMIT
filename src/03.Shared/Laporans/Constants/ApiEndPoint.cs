namespace Pertamina.SIMIT.Shared.Laporans.Constants;
public class ApiEndPoint
{
    public static class V1
    {
        public static class Laporans
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Laporans)}";

            public static class RouteTemplateFor
            {
                public const string LaporanId = "{laporanId:guid}";
                public const string Download = "Download/{laporanId:guid}";
            }
        }
    }
}
