namespace Pertamina.SIMIT.Shared.Logbooks.Constants;
public class ApiEndPoint
{
    public static class V1
    {
        public static class Logbooks
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Logbooks)}";

            public static class RouteTemplateFor
            {
                public const string LogbookId = "{logbookId:guid}";
                public const string MahasiswaId = "{mahasiswaId:guid}";
                public const string List = nameof(List);
            }
        }
    }
}
