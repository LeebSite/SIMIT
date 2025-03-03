﻿namespace Pertamina.SIMIT.Shared.Mahasiswas.Constants;
public class ApiEndpoint
{
    public static class V1
    {
        public static class Mahasiswas
        {
            public const string Segment = $"{nameof(V1)}/{nameof(Mahasiswas)}";

            public static class RouteTemplateFor
            {
                public const string MahasiswaId = "{mahasiswaId:guid}";

                public const string UpdateMahasiswas = nameof(UpdateMahasiswas);
                public const string List = nameof(List);
                public const string Count = nameof(Count);
            }
        }
    }
}
