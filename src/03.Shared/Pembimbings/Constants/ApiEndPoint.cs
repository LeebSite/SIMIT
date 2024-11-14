namespace Pertamina.SIMIT.Shared.Pembimbings.Constants;

public class ApiEndPoint
{
    public static class V1
    {
        public static class Pembimbings
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Pembimbings)}";

            public static class RouteTemplateFor
            {
                public const string PembimbingId = "{pembimbingId:guid}";
                public const string UpdatePembimbings = nameof(UpdatePembimbings);
                public const string List = nameof(List);
            }
        }
    }
}
