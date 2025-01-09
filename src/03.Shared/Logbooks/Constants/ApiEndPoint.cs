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
                // Rute untuk Logbook berdasarkan logbookId
                public const string ByLogbookId = $"{Segment}/{nameof(ByLogbookId)}/{{logbookId:guid}}";

                // Rute untuk Logbook berdasarkan mahasiswaId
                public const string ByMahasiswaId = $"{Segment}/{nameof(ByMahasiswaId)}/{{mahasiswaId:guid}}";

                // Rute untuk mendapatkan daftar logbook
                public const string List = $"{Segment}/{nameof(List)}";

                public const string Approval = $"{Segment}/{nameof(Approval)}";

                public const string ApproveSingle = "{logbookId:guid}";

                public const string MahasiswaId = "{mahasiswaId:guid}";

                public const string ApproveMultiple = $"{Segment}/{nameof(ApproveMultiple)}";

                public const string Count = nameof(Count);
            }
        }
    }
}

